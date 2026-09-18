// infinite.js — доп. задание "Средний": подгрузка следующей страницы товаров
// при прокрутке до конца каталога.

(function () {
    var grid = document.getElementById('catalogGrid');
    var sentinel = document.getElementById('sentinel');
    var spinner = document.getElementById('loadMoreSpinner');

    if (!grid || !sentinel) {
        return; // на этой странице подгружать нечего (товаров меньше одной страницы)
    }

    // Тот же тег, что выбран в фильтре — чтобы подгружались товары из той же категории
    var currentTag = new URLSearchParams(window.location.search).get('tag') || '';

    var currentPage = 1;
    var isLoading = false;
    var hasMore = true;

    var observer = new IntersectionObserver(async function (entries) {
        if (!entries[0].isIntersecting || isLoading || !hasMore) {
            return;
        }

        isLoading = true;
        currentPage++;
        if (spinner) {
            spinner.classList.remove('d-none');
        }

        try {
            var url = '/Catalog/LoadMore?page=' + currentPage + '&tag=' + encodeURIComponent(currentTag);
            var response = await fetch(url);
            var html = await response.text();

            if (html.trim()) {
                // insertAdjacentHTML не разрушает уже отрисованные карточки
                // (в отличие от innerHTML += ...), поэтому дешевле для DOM
                grid.insertAdjacentHTML('beforeend', html);
            } else {
                // Пустой ответ — товары закончились, дальше наблюдать не нужно
                hasMore = false;
                observer.disconnect();
                sentinel.style.display = 'none';
            }
        } catch (error) {
            console.error('Не удалось подгрузить товары:', error);
            hasMore = false;
            observer.disconnect();
        } finally {
            isLoading = false;
            if (spinner) {
                spinner.classList.add('d-none');
            }
        }
    });

    observer.observe(sentinel);
})();
