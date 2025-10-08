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
        // 1) Autorización (criterio: "se valida que el cliente tenga permiso")
        var authorized = await _repo.UserCanSeeOrderAsync(request.OrderId, request.CurrentUser, ct);
        if (!authorized)
            throw new UnauthorizedAccessException("No tienes permiso para consultar esta orden.");

        // 2) Datos (criterio: "si la orden no existe... mensaje claro")
        var order = await _repo.GetOrderWithTrackingAsync(request.OrderId, ct);
        if (order is null)
            throw new OrderNotFoundException(request.OrderId);

        // 3) Mapeo a DTO (criterio: response con orderId, status, statusHistory, total)
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

// Excepción específica para mapear a 404 en Presentation
public sealed class OrderNotFoundException : Exception
{
    public OrderNotFoundException(Guid id) : base($"La orden {id} no existe.") { }
}
