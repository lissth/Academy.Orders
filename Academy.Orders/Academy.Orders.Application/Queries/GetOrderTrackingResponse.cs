namespace Academy.OrdersTracking.Application.Queries.GetOrderTracking;

// Respuesta de la consulta GetOrderTrackingQuery.
// Contiene los datos principales de la orden y su historial de seguimiento.
public sealed class GetOrderTrackingResponse
{
    public Guid OrderId { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal Total { get; init; }

    public IReadOnlyList<ItemDto> Items { get; init; } = [];
    public IReadOnlyList<StatusHistoryDto> StatusHistory { get; init; } = [];

    // DTO anidado que representa un producto dentro de la orden.
    // Evita exponer directamente entidades del dominio.
    public sealed record ItemDto(string ProductName, int Quantity, decimal Price);
    public sealed record StatusHistoryDto(string Status, DateTime ChangedAt);
}

