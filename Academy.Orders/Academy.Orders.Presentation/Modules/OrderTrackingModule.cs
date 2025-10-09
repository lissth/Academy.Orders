using Academy.OrdersTracking.Application.Queries.GetOrderTracking;
using MediatR;
using Microsoft.AspNetCore.Builder;  // Map
using Microsoft.AspNetCore.Http;     // Results, StatusCodes    
using Microsoft.AspNetCore.Routing;  // IEndpointRouteBuilder 
using Microsoft.AspNetCore.Mvc; // FromHeader


namespace Academy.OrdersTracking.Presentation.Modules;


public static class OrderTrackingModule
{
    public static IEndpointRouteBuilder MapOrderTracking(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/v1/orders");

        group.MapGet("/{orderId:guid}", async (
            Guid orderId,
            [FromHeader(Name = "E-mailUser")] string? xUser,
            ISender sender) =>
        {
            if (orderId == Guid.Empty)
                return Results.BadRequest(new { message = "orderId inválido" }); //400

            var currentUser = string.IsNullOrWhiteSpace(xUser) ? "demo@customer" : xUser;

            try
            {
                var response = await sender.Send(new GetOrderTrackingQuery(orderId, currentUser));
                return Results.Ok(response);
            }
            catch (UnauthorizedAccessException)
            {
                return Results.Json(new { message = "No autorizado para consultar esta orden." }, statusCode: StatusCodes.Status401Unauthorized);
            }
            catch (OrderNotFoundException ex)
            {
                return Results.NotFound(new { message = ex.Message }); // 404 
            }
            catch (Exception)
            {
                return Results.Json(new { message = "Error inesperado del servidor." }, statusCode: StatusCodes.Status500InternalServerError);
            }
        })
        .WithName("GetOrderTracking")
        .WithSummary("Consulta el estado de una orden y su historial")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError);

        return app;
    }
}
