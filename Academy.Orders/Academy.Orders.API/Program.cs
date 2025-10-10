using Academy.OrdersTracking.Application;           
using Academy.OrdersTracking.Infrastructure;        
using Academy.OrdersTracking.Presentation.Modules;  
using Microsoft.EntityFrameworkCore;                


var builder = WebApplication.CreateBuilder(args);

// Infraestructura
var cs = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddInfrastructure(cs);

// Application 
builder.Services.AddApplication();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger UI 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Crear BD
    await ApplyMigrationsAsync(app);
}
// Endpoints
app.MapOrderTracking();

app.Run();

// Migracion
static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    await db.Database.MigrateAsync();   
}