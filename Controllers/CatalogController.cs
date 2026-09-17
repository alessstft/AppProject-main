using Microsoft.AspNetCore.Mvc;
using ProductShowcase.Models;

namespace ProductShowcase.Controllers;

public class CatalogController : Controller
{
    // GET /Catalog?tag=Электроника
    public IActionResult Index(string? tag)
    {
        var products = ProductRepository.GetAll();

        if (!string.IsNullOrWhiteSpace(tag))
        {
            products = products.Where(p => p.Category == tag).ToList();
        }

        ViewBag.Tags = ProductRepository.GetCategories();
        ViewBag.SelectedTag = tag;

        return View(products);
    }

    // POST /Catalog/AddToCart/3
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddToCart(int id, string? tag)
    {
        var product = ProductRepository.GetById(id);
        if (product != null && product.InStock)
        {
            CartStore.Add(product);
            TempData["ItemAdded"] = true;
        }

        return RedirectToAction(nameof(Index), new { tag });
    }
}
