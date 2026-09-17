namespace DashboardAdmin.Models;

// Модель одной строки сводки по тренду, используется ViewComponent'ом
// "TrendSummary" (задание 5)
public class TrendSummaryItem
{
    public Trend Trend { get; set; }
    public int Count { get; set; }
    public decimal Sum { get; set; }

    public string TrendName => Trend switch
    {
        Trend.Up => "Рост",
        Trend.Down => "Снижение",
        Trend.Stable => "Стабильно",
        _ => "Неизвестно"
    };
}
