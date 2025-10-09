using Academy.OrdersTracking.Domain.Entities;

namespace Academy.OrdersTracking.Domain.Repositories;

public interface IOrderTrackingRepository
{
    // Trae la orden con Items y StatusHistory 
    Task<Order?> GetOrderWithTrackingAsync(Guid orderId, CancellationToken ct = default);

    // Autorización para saber si el usuario puede ver la orden
    Task<bool> UserCanSeeOrderAsync(Guid orderId, string currentUser, CancellationToken ct = default);
}
