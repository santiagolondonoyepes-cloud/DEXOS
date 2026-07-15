using DEXOS.Infrastructure.Persistence;
using DEXOS.Orders.Abstractions;
using DEXOS.Orders.Models;
using Microsoft.EntityFrameworkCore;

namespace DEXOS.Infrastructure.Repositories.Orders;

/// <summary>
/// Repositorio EF Core para ordenesde cocina (KitchenOrder).
/// </summary>
public sealed class KitchenOrderRepository : IKitchenOrderRepository
{
    private readonly DexosDbContext _dbContext;

    public KitchenOrderRepository(DexosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<KitchenOrder> AddAsync(KitchenOrder order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);
        await _dbContext.KitchenOrders.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<KitchenOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.KitchenOrders.SingleOrDefaultAsync(k => k.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyCollection<KitchenOrder>> GetByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.KitchenOrders
            .Where(k => k.TenantId == tenantId)
            .OrderBy(k => k.Status)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(KitchenOrder order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);
        _dbContext.KitchenOrders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
