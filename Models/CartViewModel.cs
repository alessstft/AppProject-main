namespace ProductShowcase.Models;

public class CartViewModel
{
    public List<CartItem> Items { get; set; } = new();
    public decimal TotalAmount { get; set; }

    // Флаги для показа alert-компонентов Bootstrap (Задание 4)
    public bool ItemAdded { get; set; }
    public bool OrderConfirmed { get; set; }

    public bool IsEmpty => Items.Count == 0;
}
