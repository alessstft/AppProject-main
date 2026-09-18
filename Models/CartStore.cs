namespace ProductShowcase.Models;

/// <summary>
/// Упрощённое хранилище корзины в памяти процесса — достаточно для учебной задачи
/// по вёрстке/Bootstrap. В реальном проекте корзину привязывают к сессии/пользователю.
/// </summary>
public static class CartStore
{
    public static List<CartItem> Items { get; } = new();

    public static decimal TotalAmount => Items.Sum(i => i.LineTotal);

    public static void Add(Product product)
    {
        var existing = Items.FirstOrDefault(i => i.ProductId == product.Id);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            Items.Add(new CartItem
            {
                ProductId = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = 1
            });
        }
    }

    public static void Clear() => Items.Clear();
}
