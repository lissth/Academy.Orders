using Academy.OrdersTracking.Domain.Entities;
using Academy.OrdersTracking.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Academy.OrdersTracking.Infrastructure;

public class OrderRepository : IOrderTrackingRepository
{
    private readonly OrdersDbContext _db;

    public OrderRepository(OrdersDbContext db) => _db = db;

    // Orden con Items e Historial 
    public async Task<Order?> GetOrderWithTrackingAsync(Guid orderId, CancellationToken ct = default)
    {
        return await _db.Orders
            .Include(o => o.Items)
            .Include(o => o.StatusHistory)
            .AsNoTracking() // lectura
            .FirstOrDefaultAsync(o => o.Id == orderId, ct);
    }

    // Validación de permisos el usuario de la orden debe coincidir con el usuario registrado
    public async Task<bool> UserCanSeeOrderAsync(Guid orderId, string currentUser, CancellationToken ct = default)
    {
        return await _db.Orders
            .AnyAsync(o => o.Id == orderId && o.CustomerName == currentUser, ct);
    }
}
