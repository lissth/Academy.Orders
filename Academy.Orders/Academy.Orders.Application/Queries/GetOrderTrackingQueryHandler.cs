using Academy.OrdersTracking.Domain.Repositories;
using MediatR;

namespace Academy.OrdersTracking.Application.Queries.GetOrderTracking;

// Manejador que procesa la consulta para obtener una orden y su historial de seguimiento
// Implementa el patrón IRequestHandler
public sealed class GetOrderTrackingQueryHandler
    : IRequestHandler<GetOrderTrackingQuery, GetOrderTrackingResponse>
{
    private readonly IOrderTrackingRepository _repo;

    // Inyecta la dependencia del repositorio de órdenes
    public GetOrderTrackingQueryHandler(IOrderTrackingRepository repo)
    {
        _repo = repo;
    }

    public async Task<GetOrderTrackingResponse> Handle(GetOrderTrackingQuery request, CancellationToken ct)
    {
        // Buscar la orden en el repositorio
        Domain.Entities.Order? order = await _repo.GetOrderWithTrackingAsync(request.OrderId, ct);
        if (order is null)
            throw new OrderNotFoundException(request.OrderId); // 404

        // Verificar que el usuario tenga permiso para acceder a la orden
        var authorized = string.Equals(order.CustomerName, request.CurrentUser, StringComparison.OrdinalIgnoreCase);
        if (!authorized)
            throw new UnauthorizedAccessException("No tienes permiso para consultar esta orden."); // 401

        // Mapea los datos del dominio al DTO de respuesta
        return new GetOrderTrackingResponse
        {
            OrderId = order.Id,
            Status = order.Status,
            Total = order.Total,

            Items = order.Items
                .Select(i => new GetOrderTrackingResponse.ItemDto(i.ProductName, i.Quantity, i.Price))
                .ToList(),

            // Se ordena el historial de estados por fecha y se mapea a DTO
            StatusHistory = order.StatusHistory
                .OrderBy(h => h.ChangedAt)
                .Select(h => new GetOrderTrackingResponse.StatusHistoryDto(h.Status, h.ChangedAt))
                .ToList()
        };
    }
}
