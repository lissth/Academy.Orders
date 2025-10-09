using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Academy.OrdersTracking.Application;

// Clase para registrar los servicios de la capa Application.
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Registra todos los comandos y queries definidos en Application
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}
