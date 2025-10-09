using Academy.OrdersTracking.Application;           // AddApplication()
using Academy.OrdersTracking.Infrastructure;        // AddInfrastructure() + OrdersDbContext
using Academy.OrdersTracking.Presentation.Modules;  // MapOrderTracking
using Microsoft.EntityFrameworkCore;                // Migrate()


var builder = WebApplication.CreateBuilder(args);

// 1) Infraestructura: cadena de conexión desde appsettings.json
var cs = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddInfrastructure(cs);

// 2) Application (MediatR)
builder.Services.AddApplication();

// 3) Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // 4) Crear BD / aplicar migraciones
    await ApplyMigrationsAsync(app);
}
// 5) Endpoints
app.MapOrderTracking();

app.Run();

// Migracion
static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    await db.Database.MigrateAsync();   // crea la BD si no existe y aplica Migraciones pendientes
}