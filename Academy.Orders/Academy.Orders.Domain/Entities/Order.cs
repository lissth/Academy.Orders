using Academy.OrdersTracking.Domain.Entities;

namespace Academy.OrdersTracking.Domain.Entities;

public class Order
{
    // Id de la orden
    public Guid Id { get; private set; }

    // Usuario de la orden
    public string CustomerName { get; private set; } = string.Empty;

    // Status (created|pending|confirmed|shipped|delivered|canceled)
    public string Status { get; private set; } = "created";

    // Total  a pagar por los items
    public decimal Total { get; private set; }

    // Colecciones para mantener encapsulamiento
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items;

    private readonly List<OrderStatusHistory> _statusHistory = new();
    public IReadOnlyCollection<OrderStatusHistory> StatusHistory => _statusHistory;

    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    // Constructor requerido por EF Core
    private Order() { }

    public Order(Guid id, string customerName)
    {
        Id = id;
        CustomerName = customerName;
    }

    // Agrega un producto y recalcula el total
    public void AddItem(string productName, int quantity, decimal price)
    {
        _items.Add(new OrderItem(Guid.NewGuid(), Id, productName, quantity, price));
        RecalculateTotal();
    }

    // Cambia estado y registra una entrada en el historial
    public void ChangeStatus(string newStatus, DateTime changedAtUtc)
    {
        Status = newStatus;
        _statusHistory.Add(new OrderStatusHistory(Guid.NewGuid(), Id, newStatus, changedAtUtc));
        UpdatedAt = DateTime.UtcNow;
    }

    private void RecalculateTotal() => Total = _items.Sum(i => i.Price * i.Quantity);
}
