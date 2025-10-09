using Academy.OrdersTracking.Domain.Repositories;
using MediatR;

namespace Academy.OrdersTracking.Application.Queries.GetOrderTracking;

public sealed class GetOrderTrackingQueryHandler
    : IRequestHandler<GetOrderTrackingQuery, GetOrderTrackingResponse>
{
    private readonly IOrderTrackingRepository _repo;

    public GetOrderTrackingQueryHandler(IOrderTrackingRepository repo)
    {
        _repo = repo;
    }

    public async Task<GetOrderTrackingResponse> Handle(GetOrderTrackingQuery request, CancellationToken ct)
    {
        // 1) Buscar 
        var order = await _repo.GetOrderWithTrackingAsync(request.OrderId, ct);
        if (order is null)
            throw new OrderNotFoundException(request.OrderId); // -> 404

        // 2) Autorizar 
        var authorized = string.Equals(order.CustomerName, request.CurrentUser, StringComparison.OrdinalIgnoreCase);
        if (!authorized)
            throw new UnauthorizedAccessException("No tienes permiso para consultar esta orden."); // -> 401

        // 3) Mapear DTO
        return new GetOrderTrackingResponse
        {
            OrderId = order.Id,
            Status = order.Status,
            Total = order.Total,
            Items = order.Items
                .Select(i => new GetOrderTrackingResponse.ItemDto(i.ProductName, i.Quantity, i.Price))
                .ToList(),
            StatusHistory = order.StatusHistory
                .OrderBy(h => h.ChangedAt)
                .Select(h => new GetOrderTrackingResponse.StatusHistoryDto(h.Status, h.ChangedAt))
                .ToList()
        };
    }
}
