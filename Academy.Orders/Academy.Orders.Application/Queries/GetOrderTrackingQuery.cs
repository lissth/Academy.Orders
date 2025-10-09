using MediatR;

namespace Academy.OrdersTracking.Application.Queries.GetOrderTracking;

// Utiliza el OrderId y el usuario para validar permisos
public sealed record GetOrderTrackingQuery(Guid OrderId, string CurrentUser)
    : IRequest<GetOrderTrackingResponse>;

