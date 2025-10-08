namespace Academy.OrdersTracking.Domain.Entities;

public class OrderStatusHistory
{
    public Guid Id { get; private set; }
    public Guid OrderId { get; private set; }
    public string Status { get; private set; } = string.Empty;
    public DateTime ChangedAt { get; private set; }

    private OrderStatusHistory() { }

    public OrderStatusHistory(Guid id, Guid orderId, string status, DateTime changedAt)
    {
        Id = id;
        OrderId = orderId;
        Status = status;
        ChangedAt = changedAt;
    }
}
