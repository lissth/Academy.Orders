using Academy.OrdersTracking.Domain.Entities;

namespace Academy.OrdersTracking.Domain.Repositories;

public interface IOrderTrackingRepository
{
    // Trae la orden con Items + StatusHistory (o null si no existe)
    Task<Order?> GetOrderWithTrackingAsync(Guid orderId, CancellationToken ct = default);

    // Regla simple de autorización: ¿el usuario actual puede ver esta orden?
    Task<bool> UserCanSeeOrderAsync(Guid orderId, string currentUser, CancellationToken ct = default);
}
