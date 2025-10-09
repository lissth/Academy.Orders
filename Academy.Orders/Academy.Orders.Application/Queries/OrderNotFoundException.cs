namespace Academy.OrdersTracking.Application.Queries.GetOrderTracking;

// Excepción personalizada que para indicar que la orden no fue encontrada
public sealed class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid orderId)
        : base($"La orden {orderId} no existe.") { }
}

