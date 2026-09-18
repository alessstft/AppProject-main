namespace ProductShowcase.Models;

/// <summary>
/// Демо-отзывы для доп. задания "Сложный" (модалка с Promise.all).
/// У части товаров отзывов нет специально — чтобы показать, что модалка
/// не ломается, если один из двух параллельных запросов вернул пустой результат.
/// </summary>
public static class ReviewRepository
{
    private static readonly Dictionary<int, List<Review>> ReviewsByProductId = new()
    {
        [1] = new List<Review>
        {
            new Review { Author = "Игорь", Text = "Звук отличный, шумодав реально работает в метро.", Rating = 5 },
            new Review { Author = "Марина", Text = "Заряда хватает на пару дней при активном использовании.", Rating = 4 },
        },
        [3] = new List<Review>
        {
            new Review { Author = "Дмитрий", Text = "Брал на дачу — влаги не боится, звук громкий.", Rating = 4 },
        },
        [4] = new List<Review>
        {
            new Review { Author = "Ольга", Text = "Село точно по размеру, ткань приятная и плотная.", Rating = 5 },
            new Review { Author = "Сергей", Text = "Хорошее худи за свои деньги.", Rating = 4 },
        },
        [7] = new List<Review>
        {
            new Review { Author = "Алексей", Text = "Классика жанра, всем разработчикам к прочтению.", Rating = 5 },
            new Review { Author = "Наталья", Text = "Немного затянуто, но полезно.", Rating = 3 },
        },
        [9] = new List<Review>
        {
            new Review { Author = "Павел", Text = "Хорошая структура, примеры на актуальном коде.", Rating = 5 },
        },
    };

    /// <summary>Товары 2, 5, 6, 8 намеренно без отзывов — для проверки устойчивости модалки.</summary>
    public static List<Review> GetByProductId(int productId) =>
        ReviewsByProductId.TryGetValue(productId, out var reviews) ? reviews : new List<Review>();
}
