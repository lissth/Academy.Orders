using Academy.OrdersTracking.Application;                 // AddApplication()
using Academy.OrdersTracking.Infrastructure;              // AddInfrastructure() + OrdersDbContext
using Academy.OrdersTracking.Presentation.Modules;        // MapOrderTracking

using Academy.OrdersTracking.Domain.Entities;             // Order, OrderItem, OrderStatusHistory
using Microsoft.EntityFrameworkCore;                      // EnsureCreated / AnyAsync

var builder = WebApplication.CreateBuilder(args);

// 1) Infraestructura: cadena de conexión desde appsettings.json
var cs = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddInfrastructure(cs);

// 2) Application (MediatR)
builder.Services.AddApplication();

// 3) Swagger para pruebas
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- SOLO PARA PRUEBA SIN MIGRACIONES ---
if (app.Environment.IsDevelopment())
{
    await DevEnsureCreatedAndSeedAsync(app);
}
// ----------------------------------------

// 4) Endpoints de Presentation
app.MapOrderTracking();

app.Run();


// =================== Helpers de DEV ===================
static async Task DevEnsureCreatedAndSeedAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

    // Crea la BD y tablas si no existen (sin migraciones)
    await db.Database.EnsureCreatedAsync();

    // Si no hay órdenes, agrega 1 de prueba
    if (!await db.Orders.AnyAsync())
    {
        var orderId = Guid.NewGuid();

        var o = new Order(orderId, customerName: "demo@customer");
        o.AddItem("Doritos Nacho 150g", 2, 38.50m);
        o.AddItem("Refresco 600ml", 1, 20.00m);
        o.ChangeStatus("created", DateTime.UtcNow.AddMinutes(-30));
        o.ChangeStatus("confirmed", DateTime.UtcNow.AddMinutes(-20));
        o.ChangeStatus("shipped", DateTime.UtcNow.AddMinutes(-10));

        db.Add(o);
        await db.SaveChangesAsync();

        Console.WriteLine("===============================================");
        Console.WriteLine($"ORDER ID DE PRUEBA → {orderId}");
        Console.WriteLine("Header para autorización: X-User: demo@customer");
        Console.WriteLine("===============================================");
    }
}