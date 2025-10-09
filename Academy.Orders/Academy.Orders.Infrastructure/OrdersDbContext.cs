using Academy.OrdersTracking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Academy.OrdersTracking.Infrastructure;

public class OrdersDbContext : DbContext
{
    public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options) { }

    // Tablas
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatusHistory> OrderStatusHistory => Set<OrderStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Orders
        modelBuilder.Entity<Order>(e =>
        {
            e.ToTable("Orders");
            e.HasKey(x => x.Id);

            e.Property(x => x.CustomerName).HasMaxLength(100).IsRequired();
            e.Property(x => x.Status).HasMaxLength(50).IsRequired();
            e.Property(x => x.Total).HasColumnType("decimal(18,2)");

            // Relacion 1:N 
            e.HasMany(x => x.Items)
                .WithOne()
                .HasForeignKey(i => i.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasMany(x => x.StatusHistory)
                .WithOne()
                .HasForeignKey(h => h.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderItems 
        modelBuilder.Entity<OrderItem>(e =>
        {
            e.ToTable("OrderItems");
            e.HasKey(x => x.Id);

            e.Property(x => x.ProductName).HasMaxLength(150).IsRequired();
            e.Property(x => x.Price).HasColumnType("decimal(18,2)");
        });

        // OrderStatusHistory 
        modelBuilder.Entity<OrderStatusHistory>(e =>
        {
            e.ToTable("OrderStatusHistory");
            e.HasKey(x => x.Id);

            e.Property(x => x.Status).HasMaxLength(50).IsRequired();
        });
    }
}

