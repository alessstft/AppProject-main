# Витрина товаров с админ-панелью (CSS + Bootstrap в ASP.NET Core)

Учебный проект по практической работе «Витрина товаров с админ-панелью».
Стек: ASP.NET Core MVC (.NET 8), Bootstrap 5.3, BuildBundlerMinifier.

## Как запустить

```bash
dotnet restore
dotnet build      # соберёт bundleconfig.json -> site.min.css / site.min.js
dotnet run
```

По умолчанию открывается каталог товаров: `/Catalog` (он же `/`).

> В проекте нет базы данных — товары и корзина хранятся в памяти
> (`Models/ProductRepository.cs`, `Models/CartStore.cs`), это осознанное
> упрощение, чтобы фокус оставался на CSS/Bootstrap, а не на EF Core.
> Корзина общая на всё приложение (без сессий/авторизации) — тоже
> сознательное упрощение под масштаб практической работы.

## Что где реализовано

| Задание | Где искать |
|---|---|
| 1. Static files, `wwwroot`, `<environment>`, bundling | `Program.cs` (`UseStaticFiles`), `Views/Shared/_Layout.cshtml`, `bundleconfig.json`, `libman.json` |
| 2. Navbar (тёмная тема, бургер-меню, «Войти») | `Views/Shared/_Layout.cshtml` |
| 3. Grid + карточки товаров, адаптивность `col-12 col-md-6 col-lg-4` | `Views/Catalog/Index.cshtml`, `Controllers/CatalogController.cs` |
| 4. Alert (`alert-warning` / `alert-success`, кнопка закрытия + JS) | `Views/Shared/_AlertPartial.cshtml`, `wwwroot/js/site.js`, используется в `Catalog/Index.cshtml` и `Cart/Index.cshtml` |
| 5. Modal «Подтверждение заказа» | `Views/Cart/Index.cshtml`, `Controllers/CartController.cs` (`ConfirmOrder`) |
| 6. Кастомные стили `site.css` (не ломающие Bootstrap) | `wwwroot/css/site.css` → минифицируется в `site.min.css` |
| Доп. «Лёгкий»: адаптивная таблица цен | блок `.price-table` в `Views/Catalog/Index.cshtml` |
| Доп. «Средний»: тег-фильтр (pill-кнопки, active, JS-подсветка) | панель фильтра в `Views/Catalog/Index.cshtml` + `CatalogController.Index(string tag)` + `site.js` |
| Доп. «Сложный»: динамические классы/состояния карточек | блок `@{ borderClass / stateBadgeText ... }` в `Views/Catalog/Index.cshtml` (использует `Product.InStock`, `IsBigDiscount`, `IsHit`) |

## Структура проекта

```
ProductShowcase/
├── Controllers/
│   ├── CatalogController.cs
│   └── CartController.cs
├── Models/
│   ├── Product.cs
│   ├── ProductRepository.cs   (демо-данные, 9 товаров / 3 категории)
│   ├── CartItem.cs
│   ├── CartStore.cs
│   └── CartViewModel.cs
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml
│   │   └── _AlertPartial.cshtml
│   ├── Catalog/Index.cshtml
│   ├── Cart/Index.cshtml
│   ├── _ViewImports.cshtml
│   └── _ViewStart.cshtml
├── wwwroot/
│   ├── css/site.css, site.min.css
│   ├── js/site.js, site.min.js
│   ├── img/placeholder.svg
│   └── lib/bootstrap/dist/... (css + js, обычные и .min)
├── bundleconfig.json
├── libman.json
├── Program.cs
├── appsettings.json
└── ProductShowcase.csproj
```

## Демо-сценарий для проверки

1. `/Catalog` — сетка карточек, фильтр по тегам («Электроника» / «Одежда» / «Книги»),
   у товара «Портативная колонка BoomBox Mini» и «Кроссовки UrbanRun» — состояние
   «Нет в наличии» (красная рамка, кнопка недоступна), у товаров со скидкой >30% —
   зелёная рамка и бейдж «Большая скидка», у хитов — жёлтый бейдж «ХИТ» и тень.
2. Нажать «В корзину» у любого доступного товара → редирект на `/Catalog` с
   зелёным alert «Товар добавлен в корзину» (закрывается по кнопке или сам через 4 сек).
3. Перейти в `/Cart` → список товаров, кнопка «Оформить заказ» → модалка со сводкой
   заказа и суммой → «Подтвердить» очищает корзину и показывает alert об успехе,
   «Отмена» просто закрывает модалку.
4. Очистить корзину и открыть `/Cart` снова → `alert-warning` «Корзина пуста».
