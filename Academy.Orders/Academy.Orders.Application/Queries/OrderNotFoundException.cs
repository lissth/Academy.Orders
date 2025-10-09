namespace Academy.OrdersTracking.Application.Queries.GetOrderTracking;

public sealed class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid orderId)
        : base($"La orden {orderId} no existe.") { }
}

