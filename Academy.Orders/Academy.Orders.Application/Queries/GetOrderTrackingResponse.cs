namespace Academy.OrdersTracking.Application.Queries.GetOrderTracking;

public sealed class GetOrderTrackingResponse
{
    public Guid OrderId { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal Total { get; init; }

    public IReadOnlyList<ItemDto> Items { get; init; } = [];
    public IReadOnlyList<StatusHistoryDto> StatusHistory { get; init; } = [];

    public sealed record ItemDto(string ProductName, int Quantity, decimal Price);
    public sealed record StatusHistoryDto(string Status, DateTime ChangedAt);
}

