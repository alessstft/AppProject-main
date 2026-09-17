using DashboardAdmin.Models;

namespace DashboardAdmin.Services;

// Задание 1, п.2
public interface IDashboardRepository
{
    IEnumerable<DashboardCard> GetAllCards();
    DashboardCard? GetById(int id);
    IEnumerable<DashboardCard> GetLatest(int count);
}
