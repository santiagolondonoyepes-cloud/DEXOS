using System.Linq.Expressions;
using DEXOS.Application.Abstractions;
using DEXOS.CRM.Models;
using DEXOS.Domain.Entities;
using DEXOS.Infrastructure.Persistence.Mappings;
using DEXOS.Inventory.Models;
using DEXOS.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DEXOS.Infrastructure.Persistence;

public sealed class DexosDbContext : DbContext
{
    private readonly ITenantContext _tenantContext;

    public DexosDbContext(DbContextOptions<DexosDbContext> options, ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    private Guid CurrentTenantId => _tenantContext.TenantId ?? Guid.Empty;

    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderItem> OrderItems { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<InventoryMovement> InventoryMovements { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<CustomerPurchase> CustomerPurchases { get; set; } = null!;
    public DbSet<LoyaltyCoupon> LoyaltyCoupons { get; set; } = null!;
    public DbSet<KitchenOrder> KitchenOrders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Aplicar configuraciones de mapeo
        modelBuilder.ApplyConfiguration(new OrderMapping());
        modelBuilder.ApplyConfiguration(new OrderItemMapping());
        modelBuilder.ApplyConfiguration(new ProductMapping());
        modelBuilder.ApplyConfiguration(new InventoryMovementMapping());
        modelBuilder.ApplyConfiguration(new CustomerMapping());
        modelBuilder.ApplyConfiguration(new CustomerPurchaseMapping());
        modelBuilder.ApplyConfiguration(new LoyaltyCouponMapping());
        modelBuilder.ApplyConfiguration(new KitchenOrderMapping());

        ApplyTenantIsolationQueryFilters(modelBuilder);
    }

    private void ApplyTenantIsolationQueryFilters(ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var tenantProperty = entityType.FindProperty("TenantId");
            if (tenantProperty is null || tenantProperty.ClrType != typeof(Guid))
            {
                continue;
            }

            var parameter = Expression.Parameter(entityType.ClrType, "entity");

            var tenantPropertyAccess = Expression.Call(
                typeof(EF),
                nameof(EF.Property),
                new[] { typeof(Guid) },
                parameter,
                Expression.Constant("TenantId"));

            var currentTenantProperty = Expression.Property(Expression.Constant(this), nameof(CurrentTenantId));
            var tenantNotSet = Expression.Equal(currentTenantProperty, Expression.Constant(Guid.Empty));
            var tenantMatch = Expression.Equal(tenantPropertyAccess, currentTenantProperty);
            var body = Expression.OrElse(tenantNotSet, tenantMatch);

            entityType.SetQueryFilter(Expression.Lambda(body, parameter));
        }
    }
}
