// cart.js — добавление товара в корзину через AJAX (без перезагрузки страницы).
// Подключается только на странице каталога.

(function () {
    var buttons = document.querySelectorAll('.add-to-cart');

    buttons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            // data-product-id="5" → dataset.productId = "5"
            var productId = this.dataset.productId;
            addToCart(productId, this);
        });
    });

    async function addToCart(productId, button) {
        var originalText = button.textContent;

        // Блокируем кнопку — без этого повторный быстрый клик отправит
        // несколько одинаковых запросов, пока первый ещё не ответил.
        button.disabled = true;
        button.innerHTML = '<span class="spinner-border spinner-border-sm"></span> Добавление...';

        try {
            // POST, а не GET: запрос изменяет состояние на сервере (кладёт товар
            // в корзину), а GET-запросы по конвенции должны быть безопасными
            // (ничего не менять) — их браузер может закэшировать или повторить.
            var response = await fetch('/Catalog/AddToCart', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                },
                body: 'id=' + encodeURIComponent(productId)
            });

            if (!response.ok) {
                throw new Error('HTTP ' + response.status);
            }

            var data = await response.json();

            if (data.success) {
                // Обновляем бейдж корзины в navbar.
                // data.cartCount — общее количество единиц товара в корзине
                // (сервер уже посчитал сумму Quantity по всем позициям).
                var badge = document.getElementById('cartBadge');
                if (badge) {
                    badge.textContent = data.cartCount;
                }

                // Доп. задание "Лёгкий": вместо смены текста кнопки на "Добавлено ✓"
                // показываем toast-уведомление с названием товара.
                showCartToast(data.productName);

                setTimeout(function () {
                    button.textContent = originalText;
                    button.disabled = false;
                }, 1500);
            } else {
                alert('Ошибка: ' + data.message);
                button.textContent = originalText;
                button.disabled = false;
            }
        } catch (error) {
            console.error(error);
            button.textContent = 'Ошибка';
            setTimeout(function () {
                button.textContent = originalText;
                button.disabled = false;
            }, 2000);
        }
    }

    function showCartToast(productName) {
        var toastEl = document.getElementById('cartToast');
        if (!toastEl) {
            return;
        }

        document.getElementById('cartToastText').textContent =
            'Товар «' + productName + '» добавлен в корзину';

        var toast = bootstrap.Toast.getOrCreateInstance(toastEl, { delay: 3000 });
        toast.show();
    }
})();
