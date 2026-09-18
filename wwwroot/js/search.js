// search.js — живой поиск по каталогу без перезагрузки страницы.
// Подключается только на странице каталога (Views/Catalog/Index.cshtml, @section Scripts).

(function () {
    var input = document.getElementById('searchInput');
    var grid = document.getElementById('catalogGrid');

    if (!input || !grid) {
        return; // на этой странице поиска нет — выходим тихо
    }

    // Текущий тег-фильтр берём из URL (?tag=Электроника), чтобы поиск
    // работал внутри уже выбранной категории, а не сбрасывал её.
    var currentTag = new URLSearchParams(window.location.search).get('tag') || '';

    var timeoutId;

    input.addEventListener('input', function () {
        // Отменяем предыдущий отложенный запрос — пользователь продолжает печатать
        clearTimeout(timeoutId);

        var query = input.value.trim();

        if (query.length === 0) {
            // Поле полностью очищено — возвращаемся к обычному (серверному) списку каталога
            window.location.reload();
            return;
        }

        if (query.length < 2) {
            // 1 символ — ждём, пока пользователь введёт хотя бы 2, ничего не делаем
            return;
        }

        // Дебаунс 300 мс: запрос уходит только когда пользователь перестал печатать
        timeoutId = setTimeout(function () {
            searchProducts(query);
        }, 300);
    });

    async function searchProducts(query) {
        // Прячем сентинель бесконечной подгрузки на время поиска — иначе он может
        // остаться внизу страницы и подгрузить "обычные" товары поверх результатов поиска.
        var sentinel = document.getElementById('sentinel');
        if (sentinel) {
            sentinel.style.display = 'none';
        }

        // 1. Показать спиннер вместо каталога на время запроса
        grid.innerHTML =
            '<div class="col-12 text-center py-5">' +
            '<div class="spinner-border text-primary"></div>' +
            '</div>';

        try {
            // 2. Отправить запрос к серверу.
            // encodeURIComponent обязателен: без него спецсимволы и кириллица
            // в поисковой строке сломают URL (пробелы, "&", "?", "+" и т.п.)
            var url = '/Catalog/Search?query=' + encodeURIComponent(query) +
                '&tag=' + encodeURIComponent(currentTag);

            // async/await вместо .then()-цепочек — код читается как обычный
            // последовательный, хотя запрос выполняется асинхронно.
            var response = await fetch(url);

            if (!response.ok) {
                throw new Error('HTTP ' + response.status);
            }

            // 3. Получить HTML из ответа (partial-представление без layout)
            var html = await response.text();

            // 4. Пустой ответ — сервер ничего не нашёл
            if (!html.trim()) {
                grid.innerHTML = '<div class="col-12"><div class="alert alert-info">Ничего не найдено</div></div>';
            } else {
                // 5. Вставить карточки в сетку
                grid.innerHTML = html;
            }
        } catch (error) {
            grid.innerHTML = '<div class="col-12"><div class="alert alert-danger">Ошибка поиска. Попробуйте ещё раз.</div></div>';
            console.error(error);
        }
    }
})();
