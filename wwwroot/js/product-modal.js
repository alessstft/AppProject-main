// product-modal.js — доп. задание "Сложный": модалка товара, содержимое которой
// грузится из двух параллельных AJAX-запросов (детали + отзывы) через Promise.all.

(function () {
    var buttons = document.querySelectorAll('.open-details');
    var modalEl = document.getElementById('productModal');
    var modalBody = document.getElementById('modalBody');

    if (!modalEl || !modalBody || buttons.length === 0) {
        return;
    }

    buttons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            openProductModal(this.dataset.productId);
        });
    });

    async function openProductModal(productId) {
        // 1. Сразу открываем модалку со спиннером — не заставляем ждать открытия
        modalBody.innerHTML = '<div class="text-center py-4"><div class="spinner-border"></div></div>';
        bootstrap.Modal.getOrCreateInstance(modalEl).show();

        try {
            // 2. Оба запроса уходят одновременно, а не один за другим —
            // Promise.all ждёт оба ответа параллельно, это быстрее, чем
            // await первого, а потом await второго.
            var results = await Promise.all([
                fetch('/Catalog/GetProductDetails?id=' + productId),
                fetch('/Catalog/GetProductReviews?id=' + productId)
            ]);

            var detailsResp = results[0];
            var reviewsResp = results[1];

            if (!detailsResp.ok) {
                throw new Error('HTTP ' + detailsResp.status);
            }

            // 3. Детали товара обязательны — без них показывать нечего
            var product = await detailsResp.json();

            // 4. Отзывы не критичны: если запрос не удался, товар всё равно
            // показываем, просто с пустым списком отзывов.
            var reviews = [];
            try {
                if (reviewsResp.ok) {
                    reviews = await reviewsResp.json();
                }
            } catch {
                reviews = [];
            }

            modalBody.innerHTML = renderProductModal(product, reviews);
        } catch (error) {
            console.error(error);
            modalBody.innerHTML = '<div class="alert alert-danger mb-0">Не удалось загрузить данные о товаре.</div>';
        }
    }

    function renderProductModal(product, reviews) {
        var html = '';
        html += '<div class="row g-3 align-items-start mb-3">';
        html += '  <div class="col-4"><img src="' + product.imageUrl + '" class="img-fluid rounded" alt="' + product.name + '"></div>';
        html += '  <div class="col-8">';
        html += '    <h4>' + product.name + '</h4>';
        html += '    <p class="fs-5 fw-bold text-danger mb-2">' + product.price + '</p>';
        html += '    <p class="text-muted mb-0">' + product.description + '</p>';
        html += '  </div>';
        html += '</div>';
        html += '<hr><h5>Отзывы</h5>';

        if (reviews.length > 0) {
            html += '<ul class="list-group">';
            reviews.forEach(function (r) {
                html += '<li class="list-group-item">' +
                    '<strong>' + r.author + '</strong> (' + r.rating + '/5)<br>' +
                    r.text + '</li>';
            });
            html += '</ul>';
        } else {
            html += '<p class="text-muted mb-0">Отзывов пока нет</p>';
        }

        return html;
    }
})();
