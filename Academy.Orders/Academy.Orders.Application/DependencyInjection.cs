using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Academy.OrdersTracking.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // v11: registra handlers de ESTE ensamblado
        services.AddMediatR(Assembly.GetExecutingAssembly());
        return services;
    }
}
