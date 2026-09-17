using DashboardAdmin.Models;

namespace DashboardAdmin.Services;

// Задание 1, п.3 — статический список из 6 карточек:
// разные тренды, разные единицы измерения, значения (включая одно отрицательное)
public class InMemoryDashboardRepository : IDashboardRepository
{
    private static readonly List<DashboardCard> _cards = new()
    {
        new DashboardCard
        {
            Id = 1,
            Title = "Выручка",
            Value = 2_500_000m,
            Trend = Trend.Up,
            Unit = "руб.",
            Description = "Общая выручка за текущий отчётный период"
        },
        new DashboardCard
        {
            Id = 2,
            Title = "Новые заказы",
            Value = 154,
            Trend = Trend.Up,
            Unit = "шт.",
            Description = "Количество новых заказов за месяц"
        },
        new DashboardCard
        {
            Id = 3,
            Title = "Средний чек",
            Value = 4952.50m,
            Trend = Trend.Stable,
            Unit = "руб.",
            Description = "Средняя сумма одного заказа"
        },
        new DashboardCard
        {
            Id = 4,
            Title = "Отток клиентов",
            Value = -15.4m,
            Trend = Trend.Down,
            Unit = "%",
            Description = "Изменение доли ушедших клиентов относительно прошлого месяца"
        },
        new DashboardCard
        {
            Id = 5,
            Title = "Складские остатки",
            Value = 9720,
            Trend = Trend.Down,
            Unit = "шт.",
            Description = "Остаток товара на складе на конец периода"
        },
        new DashboardCard
        {
            Id = 6,
            Title = "Конверсия сайта",
            Value = 8.7m,
            Trend = Trend.Stable,
            Unit = "%",
            Description = "Доля посетителей сайта, совершивших покупку"
        }
    };

    public IEnumerable<DashboardCard> GetAllCards() => _cards;

    public DashboardCard? GetById(int id) => _cards.FirstOrDefault(c => c.Id == id);

    public IEnumerable<DashboardCard> GetLatest(int count) =>
        _cards.OrderByDescending(c => c.Id).Take(count);
}
