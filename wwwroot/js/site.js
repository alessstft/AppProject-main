// Задание 4: JS для скрытия alert (доп. к штатной кнопке закрытия Bootstrap) —
// автоматически прячем success/warning уведомления через несколько секунд.
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.auto-dismiss-alert').forEach(function (alertEl) {
        setTimeout(function () {
            var bsAlert = bootstrap.Alert.getOrCreateInstance(alertEl);
            bsAlert.close();
        }, 4000);
    });

    // Доп. задание "Средний": тег-фильтр — мгновенная подсветка выбранного тега
    // до перезагрузки страницы (сама фильтрация выполняется на сервере по ссылке).
    var tagLinks = document.querySelectorAll('.tag-filter');
    tagLinks.forEach(function (link) {
        link.addEventListener('click', function () {
            tagLinks.forEach(function (l) {
                l.classList.remove('js-active-preview');
            });
            link.classList.add('js-active-preview');
        });
    });

    // Практическая 3, Задание 5: при загрузке ЛЮБОЙ страницы запрашиваем у сервера
    // актуальное количество товаров в корзине и обновляем бейдж в navbar.
    // Нужно потому, что бейдж — это просто текст в HTML, который сервер отрисовал
    // при последней полной загрузке страницы; после AJAX-добавлений на другой
    // странице (или в другой вкладке) он "не знает" об изменениях, пока не спросит сам.
    (async function updateCartBadgeOnLoad() {
        var badge = document.getElementById('cartBadge');
        if (!badge) {
            return;
        }

        try {
            var response = await fetch('/Catalog/GetCartCount');
            var data = await response.json();
            badge.textContent = data.count > 0 ? data.count : '0';
        } catch (error) {
            console.error('Не удалось получить количество товаров в корзине:', error);
        }
    })();
});
