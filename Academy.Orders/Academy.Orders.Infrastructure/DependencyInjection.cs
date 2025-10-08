using Academy.OrdersTracking.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Academy.OrdersTracking.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        // DbContext apuntando a SQL Server
        services.AddDbContext<OrdersDbContext>(opt =>
            opt.UseSqlServer(connectionString));

        // Repositorio
        services.AddScoped<IOrderTrackingRepository, OrderRepository>();

        return services;
    }
}

