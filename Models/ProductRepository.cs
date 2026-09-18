namespace ProductShowcase.Models;

/// <summary>
/// Простое статическое "хранилище" товаров для учебного проекта.
/// В реальном приложении здесь был бы доступ к базе данных (EF Core и т.п.).
/// </summary>
public static class ProductRepository
{
    private static readonly List<Product> Products = new()
    {
        new Product
        {
            Id = 1,
            Name = "Беспроводные наушники SoundX",
            Description = "Компактные Bluetooth-наушники с шумоподавлением.",
            Price = 3990,
            OldPrice = 5990,
            Category = "Электроника",
            Stock = 12,
            IsHit = true
        },
        new Product
        {
            Id = 2,
            Name = "Смарт-часы PulseFit",
            Description = "Мониторинг пульса, сна и активности, до 7 дней автономности.",
            Price = 6490,
            OldPrice = null,
            Category = "Электроника",
            Stock = 5,
            IsHit = false
        },
        new Product
        {
            Id = 3,
            Name = "Портативная колонка BoomBox Mini",
            Description = "Влагозащищённая колонка 10 Вт с подсветкой.",
            Price = 2190,
            OldPrice = 3490,
            Category = "Электроника",
            Stock = 0,
            IsHit = false
        },
        new Product
        {
            Id = 4,
            Name = "Худи Basic",
            Description = "Хлопковое худи прямого кроя, унисекс.",
            Price = 2490,
            OldPrice = null,
            Category = "Одежда",
            Stock = 20,
            IsHit = true
        },
        new Product
        {
            Id = 5,
            Name = "Джинсы Slim Fit",
            Description = "Классические джинсы зауженного кроя.",
            Price = 3190,
            OldPrice = 3990,
            Category = "Одежда",
            Stock = 8,
            IsHit = false
        },
        new Product
        {
            Id = 6,
            Name = "Кроссовки UrbanRun",
            Description = "Лёгкие кроссовки для города и пробежек.",
            Price = 4290,
            OldPrice = 6990,
            Category = "Одежда",
            Stock = 0,
            IsHit = false
        },
        new Product
        {
            Id = 7,
            Name = "«Чистый код» — Роберт Мартин",
            Description = "Практическое руководство по написанию качественного кода.",
            Price = 1290,
            OldPrice = null,
            Category = "Книги",
            Stock = 30,
            IsHit = true
        },
        new Product
        {
            Id = 8,
            Name = "«ASP.NET Core в действии»",
            Description = "Подробный разбор веб-разработки на ASP.NET Core.",
            Price = 1590,
            OldPrice = 1990,
            Category = "Книги",
            Stock = 4,
            IsHit = false
        },
        new Product
        {
            Id = 9,
            Name = "«Паттерны проектирования»",
            Description = "Классический сборник шаблонов проектирования ПО.",
            Price = 990,
            OldPrice = 1690,
            Category = "Книги",
            Stock = 15,
            IsHit = false
        }
    };

    public static List<Product> GetAll() => Products;

    public static Product? GetById(int id) => Products.FirstOrDefault(p => p.Id == id);

    public static IReadOnlyList<string> GetCategories() =>
        Products.Select(p => p.Category).Distinct().OrderBy(c => c).ToList();
}
