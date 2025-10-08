namespace Academy.OrdersTracking.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    private OrderItem() { }

    public OrderItem(Guid id, Guid orderId, string productName, int quantity, decimal price)
    {
        Id = id;
        OrderId = orderId;
        ProductName = productName;
        Quantity = quantity;
        Price = price;
    }
}
