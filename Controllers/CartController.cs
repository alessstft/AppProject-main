using Microsoft.AspNetCore.Mvc;
using ProductShowcase.Models;

namespace ProductShowcase.Controllers;

public class CartController : Controller
{
    // GET /Cart
    public IActionResult Index()
    {
        var vm = new CartViewModel
        {
            Items = CartStore.Items,
            TotalAmount = CartStore.TotalAmount,
            ItemAdded = TempData["ItemAdded"] as bool? ?? false,
            OrderConfirmed = TempData["OrderConfirmed"] as bool? ?? false
        };

        return View(vm);
    }

    // POST /Cart/ConfirmOrder — подтверждение заказа из модального окна
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmOrder()
    {
        CartStore.Clear();
        TempData["OrderConfirmed"] = true;
        return RedirectToAction(nameof(Index));
    }
}
