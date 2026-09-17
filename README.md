# DashboardAdmin — панель администратора с динамическими дашбордами

Проект ASP.NET Core MVC (.NET 8), реализующий практическую работу
«Панель администратора с динамическими дашбордами»: Razor-синтаксис,
Layouts, ViewComponents.

## Запуск

```bash
cd DashboardAdmin
dotnet restore
dotnet run
```

Затем открыть в браузере адрес, который выведет консоль
(обычно http://localhost:5000 или https://localhost:5001).

- `/Dashboard/Index` — обычный дашборд (layout `_AppLayout` с боковой панелью)
- `/Dashboard/Print` — печатная версия (layout `_PrintLayout`, без шапки/навигации/подвала)

## Что где реализовано

| Задание | Файл(ы) |
|---|---|
| 1. Модели и хранилище | `Models/DashboardCard.cs`, `Services/IDashboardRepository.cs`, `Services/InMemoryDashboardRepository.cs`, регистрация Singleton в `Program.cs` |
| 2. Многоуровневый Layout | `Views/Shared/_Layout.cshtml`, `_AppLayout.cshtml`, `Views/_ViewStart.cshtml`, `Views/_ViewImports.cshtml` |
| 3. Razor-логика дашборда | `Views/Dashboard/Index.cshtml` (@functions, @{ }, @section PageHeader, foreach, перенос строки, сводка) |
| 4. Partial-представление | `Views/Shared/_DashboardCard.cshtml`, вызывается из `Index.cshtml` |
| 5. ViewComponent «Сводка по трендам» | `ViewComponents/TrendSummaryViewComponent.cs`, `Views/Shared/Components/TrendSummary/Default.cshtml` |
| 6. ViewData / ViewBag | `Controllers/DashboardController.cs` (Index), вывод в `Index.cshtml` |
| Доп. «Лёгкий»: форматирование и бейджи | `GetValueClass` в `_DashboardCard.cshtml` |
| Доп. «Средний»: динамический layout | `Views/_ViewStart.cshtml`, `Views/Shared/_PrintLayout.cshtml`, `DashboardController.Print()`, секция `Actions` в `_Layout.cshtml` / `Index.cshtml` |
| Доп. «Сложный»: шаблонные делегаты | блок `renderCard` / `renderSummary` в `Index.cshtml`, метод `GetAverage` |

Пояснения по спорным местам задания (например, почему выбор Layout
вынесен в `_ViewStart.cshtml`, а не задан вручную в `Index.cshtml`, и
почему `_PrintLayout` не вкладывается в `_Layout`) даны комментариями
`@* ... *@` прямо в соответствующих файлах.
