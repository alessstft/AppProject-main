using Microsoft.AspNetCore.Mvc;
using DashboardAdmin.Services;

namespace DashboardAdmin.Controllers;

public class DashboardController : Controller
{
    private readonly IDashboardRepository _repository;

    public DashboardController(IDashboardRepository repository)
    {
        _repository = repository;
    }

    // Задание 6 — обычный режим отображения дашборда
    public IActionResult Index()
    {
        var cards = _repository.GetAllCards();

        ViewData["Title"] = "Дашборд";
        ViewData["ReportPeriod"] = "Сентябрь 2026";
        ViewBag.Currency = "RUB";

        return View(cards);
    }

    // Дополнительное задание "Средний" — печатный режим.
    // Передаём ViewData["Mode"] = "Print", по которому _ViewStart.cshtml
    // подставит упрощённый _PrintLayout вместо _AppLayout.
    public IActionResult Print()
    {
        var cards = _repository.GetAllCards();

        ViewData["Title"] = "Дашборд — печать";
        ViewData["ReportPeriod"] = "Сентябрь 2026";
        ViewData["Mode"] = "Print";
        ViewBag.Currency = "RUB";

        return View("Index", cards);
    }
}
