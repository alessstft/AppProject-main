namespace ProductShowcase.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    // Текущая (актуальная) цена
    public decimal Price { get; set; }

    // Старая цена — если задана и больше Price, товар считается со скидкой
    public decimal? OldPrice { get; set; }

    public string ImageUrl { get; set; } = "/img/placeholder.svg";

    // Категория для тег-фильтра каталога
    public string Category { get; set; } = string.Empty;

    // Остаток на складе. 0 => "Нет в наличии"
    public int Stock { get; set; }

    // Хит продаж
    public bool IsHit { get; set; }

    public bool InStock => Stock > 0;

    public bool HasDiscount => OldPrice.HasValue && OldPrice.Value > Price;

    // Процент скидки, округлённый до целого
    public int DiscountPercent =>
        HasDiscount ? (int)Math.Round((1 - Price / OldPrice!.Value) * 100) : 0;

    public bool IsBigDiscount => DiscountPercent > 30;
}
