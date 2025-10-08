using Academy.OrdersTracking.Application.Queries.GetOrderTracking;
using MediatR;
using Microsoft.AspNetCore.Builder;  // <-- MapGroup vive aquí
using Microsoft.AspNetCore.Http;     // Results, StatusCodes                                     // Results, StatusCodes
using Microsoft.AspNetCore.Routing;  // IEndpointRouteBuilder / MapGroup

namespace Academy.OrdersTracking.Presentation.Modules;

public static class OrderTrackingModule
{
    /// <summary>
    /// Registra los endpoints de Order Tracking.
    /// </summary>
    public static IEndpointRouteBuilder MapOrderTracking(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/orders");

        // GET /api/v1/orders/{orderId}
        group.MapGet("/{orderId:guid}", async (Guid orderId, ISender sender, HttpContext http) =>
        {
            // 1) Validación de ruta (diagrama: "orderId inválido o nulo")
            if (orderId == Guid.Empty)
                return Results.BadRequest(new { message = "orderId inválido" });

            // 2) Usuario actual (para demo): lo tomamos de un header. En producción vendrá del JWT.
            var currentUser = http.Request.Headers["X-User"].FirstOrDefault() ?? "demo@customer";

            try
            {
                // 3) Llamar a Application vía MediatR (diagrama: sender.Send(GetOrderTrackingQuery))
                var response = await sender.Send(new GetOrderTrackingQuery(orderId, currentUser));
                return Results.Ok(response); // 200 OK
            }
            catch (UnauthorizedAccessException)
            {
                // Criterio: se valida permiso del cliente para consultar la orden
                return Results.StatusCode(StatusCodes.Status401Unauthorized);
            }
            catch (OrderNotFoundException ex)
            {
                // Criterio: si no existe, devolver mensaje claro
                return Results.NotFound(new { message = ex.Message }); // 404
            }
            catch
            {
                // Criterio técnico: manejar 500 errores del servidor
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("GetOrderTracking")
        .WithSummary("Consulta el estado de una orden y su historial");

        return app;
    }
}
