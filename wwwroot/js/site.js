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
});
