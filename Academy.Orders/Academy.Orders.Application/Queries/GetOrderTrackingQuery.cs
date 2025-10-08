using MediatR;

namespace Academy.OrdersTracking.Application.Queries.GetOrderTracking;

// Lleva el Id de la orden y el "usuario actual" para validar permisos
public sealed record GetOrderTrackingQuery(Guid OrderId, string CurrentUser)
    : IRequest<GetOrderTrackingResponse>;

