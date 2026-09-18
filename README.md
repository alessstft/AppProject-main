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

---

# Практическая работа 3: AJAX-интерактивность каталога

Клиентский JavaScript (vanilla, без jQuery): DOM, события, Fetch API, `async`/`await`.
Серверная часть — новые методы в `CatalogController`, которые отдают не страницы,
а HTML-фрагменты или JSON.

## Важное отступление от методички: корзина без Session

В методичке предлагается хранить корзину в `Session` (`HttpContext.Session`,
список ID через запятую). В проекте с практической 2 уже есть готовая
статическая корзина — `Models/CartStore.cs` (список `CartItem` с количеством,
общая на всё приложение). Заводить рядом ещё и Session-корзину означало бы
два независимых источника правды о корзине в одном проекте, поэтому AJAX-метод
`AddToCart` в `CatalogController` переиспользует существующий `CartStore`.
Функционально результат тот же: бейдж в navbar обновляется, счётчик
запрашивается с сервера при загрузке страницы — просто без отдельного
модуля Session в `Program.cs`.

По той же причине `[ValidateAntiForgeryToken]` на AJAX-эндпоинте `AddToCart`
не используется: `fetch()` в `cart.js` не передаёт токен, а настройка полноценной
CSRF-защиты для AJAX (передача токена в заголовке запроса) выходит за рамки
практической работы.

## Что где реализовано

| Задание | Где искать |
|---|---|
| 1.1 `Search` — HTML-фрагмент | `CatalogController.Search`, `Views/Shared/_ProductList.cshtml` |
| 1.2 `AddToCart` — JSON | `CatalogController.AddToCart` (использует `CartStore`, см. выше) |
| 1.3 `GetCartCount` — JSON | `CatalogController.GetCartCount` |
| 2. Разметка (`searchInput`, `catalogGrid`, `data-product-id`, `cartBadge`) | `Views/Catalog/Index.cshtml`, `Views/Shared/_ProductCard.cshtml`, `Views/Shared/_Layout.cshtml` |
| 3. Живой поиск с дебаунсом | `wwwroot/js/search.js` |
| 4. Добавление в корзину через AJAX | `wwwroot/js/cart.js` |
| 5. Счётчик корзины при загрузке страницы | `wwwroot/js/site.js` (блок `updateCartBadgeOnLoad`) |
| 6. Подключение скриптов (глобально / через секцию) | `Views/Shared/_Layout.cshtml` (site.js — глобально), `Views/Catalog/Index.cshtml` (`@section Scripts` — search.js, cart.js, infinite.js, product-modal.js) |

**Почему `search.js`/`cart.js` подключены через `@section Scripts`, а `site.js` —
глобально:** `site.js` обновляет бейдж корзины и закрывает алерты — это нужно
на *любой* странице сайта. `search.js` и `cart.js` работают с элементами
(`#searchInput`, `.add-to-cart`), которых просто не существует нигде, кроме
страницы каталога — подключать их глобально означало бы искать
несуществующие элементы на каждой странице (лишний, бессмысленный код).

## Дополнительные задания

- **Лёгкий** — toast-уведомление (`#cartToast` в `_Layout.cshtml`) вместо
  смены текста кнопки на «Добавлено ✓»: `cart.js` → `showCartToast()`.
- **Средний** — бесконечная подгрузка каталога: `CatalogController.Index`
  отдаёт первые 6 товаров, `CatalogController.LoadMore(page, tag)` — следующие
  порции; `wwwroot/js/infinite.js` следит за `#sentinel` через
  `IntersectionObserver`. При активном поиске сентинель скрывается
  (`search.js`), чтобы подгрузка каталога не подмешивалась к результатам поиска.
- **Сложный** — модалка товара с параллельной загрузкой через `Promise.all`:
  `CatalogController.GetProductDetails` + `GetProductReviews`
  (`Models/Review.cs`, `Models/ReviewRepository.cs` — у части товаров отзывов
  специально нет, чтобы показать устойчивость к «частичному» результату),
  `wwwroot/js/product-modal.js`, кнопка «Подробнее» в `_ProductCard.cshtml`.

## Демо-сценарий для проверки

1. `/Catalog/Search?query=наушники` в адресной строке — HTML-фрагмент карточки без layout.
2. `/Catalog/GetCartCount` — `{"count":0}`.
3. На `/Catalog` ввести 2+ символа в поиск → спиннер → карточки без перезагрузки;
   стереть текст до 1 символа → страница перезагружается целиком.
4. Нажать «В корзину» → спиннер на кнопке → зелёный toast в углу экрана →
   бейдж в navbar увеличился без перезагрузки страницы.
5. Обновить страницу (F5) → бейдж сохранил значение (запрос `GetCartCount`
   при `DOMContentLoaded`).
6. Прокрутить каталог до конца → подгружаются оставшиеся товары (после 6
   показанных изначально) — виден короткий спиннер «Загрузка...».
7. Нажать «Подробнее» на карточке → модалка открывается сразу со спиннером,
   затем заполняется деталями и отзывами (или «Отзывов пока нет» — для
   товаров 2, 5, 6, 8, у которых отзывов в демо-данных нет).
