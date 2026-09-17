using DashboardAdmin.Models;
using DashboardAdmin.Services;
using Microsoft.AspNetCore.Mvc;

namespace DashboardAdmin.ViewComponents;

// Задание 5 — ViewComponent "Сводка по трендам"
public class TrendSummaryViewComponent : ViewComponent
{
    private readonly IDashboardRepository _repository;

    public TrendSummaryViewComponent(IDashboardRepository repository)
    {
        _repository = repository;
    }

    public IViewComponentResult Invoke()
    {
        var cards = _repository.GetAllCards();

        var summary = cards
            .GroupBy(c => c.Trend)
            .Select(g => new TrendSummaryItem
            {
                Trend = g.Key,
                Count = g.Count(),
                Sum = g.Sum(c => c.Value)
            })
            .OrderBy(s => s.Trend)
            .ToList();

        // Views/Shared/Components/TrendSummary/Default.cshtml
        return View(summary);
    }
}
