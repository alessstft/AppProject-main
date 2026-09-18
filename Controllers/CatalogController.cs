using Microsoft.AspNetCore.Mvc;
using ProductShowcase.Models;

namespace ProductShowcase.Controllers;

public class CatalogController : Controller
{
    // Доп. задание "Средний": показываем товары порциями по 6 штук,
    // остальное подгружается через LoadMore при прокрутке.
    private const int PageSize = 6;

    // GET /Catalog?tag=Электроника
    public IActionResult Index(string? tag)
    {
        var products = FilterByTag(ProductRepository.GetAll(), tag);

        ViewBag.Tags = ProductRepository.GetCategories();
        ViewBag.SelectedTag = tag;
        ViewBag.HasMore = products.Count > PageSize;

        return View(products.Take(PageSize).ToList());
    }

    // ------------------------------------------------------------------
    // Практическая 3, Задание 1.1: живой поиск — отдаёт HTML-фрагмент
    // (partial _ProductList), а не целую страницу.
    // Доп. к условию задания: если на странице уже выбран тег-фильтр,
    // поиск идёт внутри него же (tag прилетает от search.js из URL).
    // ------------------------------------------------------------------
    [HttpGet]
    public IActionResult Search(string? query, string? tag)
    {
        var products = FilterByTag(ProductRepository.GetAll(), tag);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var q = query.Trim().ToLower();
            products = products
                .Where(p => p.Name.ToLower().Contains(q) || p.Description.ToLower().Contains(q))
                .ToList();
        }

        return PartialView("_ProductList", products);
    }

    // ------------------------------------------------------------------
    // Доп. задание "Средний": подгрузка следующей порции товаров
    // при прокрутке (IntersectionObserver в infinite.js).
    // ------------------------------------------------------------------
    [HttpGet]
    public IActionResult LoadMore(int page, string? tag)
    {
        var products = FilterByTag(ProductRepository.GetAll(), tag);

        var pageItems = products
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .ToList();

        return PartialView("_ProductList", pageItems);
    }

    // ------------------------------------------------------------------
    // Практическая 3, Задание 1.2: добавление в корзину через AJAX — JSON вместо редиректа.
    //
    // fetch() в cart.js отправляет только id без токена 
    // ------------------------------------------------------------------
    [HttpPost]
    public IActionResult AddToCart(int id)
    {
        var product = ProductRepository.GetById(id);
        if (product is null || !product.InStock)
        {
            return Json(new { success = false, message = "Товар недоступен для добавления в корзину." });
        }

        CartStore.Add(product);

        return Json(new
        {
            success = true,
            cartCount = CartStore.Items.Sum(i => i.Quantity),
            productName = product.Name
        });
    }

    // ------------------------------------------------------------------
    // Практическая 3, Задание 1.3: количество товаров в корзине для бейджа.
    // ------------------------------------------------------------------
    [HttpGet]
    public IActionResult GetCartCount()
    {
        return Json(new { count = CartStore.Items.Sum(i => i.Quantity) });
    }

    // ------------------------------------------------------------------
    // Доп. задание "Сложный": данные для модалки товара (Promise.all).
    // ------------------------------------------------------------------
    [HttpGet]
    public IActionResult GetProductDetails(int id)
    {
        var product = ProductRepository.GetById(id);
        if (product is null)
        {
            return NotFound();
        }

        return Json(new
        {
            name = product.Name,
            price = product.Price.ToString("C0"),
            description = product.Description,
            imageUrl = product.ImageUrl
        });
    }

    [HttpGet]
    public IActionResult GetProductReviews(int id)
    {
        return Json(ReviewRepository.GetByProductId(id));
    }

    private static List<Product> FilterByTag(List<Product> products, string? tag) =>
        string.IsNullOrWhiteSpace(tag)
            ? products
            : products.Where(p => p.Category == tag).ToList();
}
