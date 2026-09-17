namespace DashboardAdmin.Models;

public enum Trend
{
    Up,
    Down,
    Stable
}

// Задание 1, п.1 — модель карточки показателя дашборда
public class DashboardCard
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public Trend Trend { get; set; }
    public string Unit { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
