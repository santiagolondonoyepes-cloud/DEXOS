using DEXOS.Application.Abstractions;
using DEXOS.Domain.Entities;
using DEXOS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DEXOS.Infrastructure.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly DexosDbContext _dbContext;

    public OrderRepository(DexosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        await _dbContext.Orders.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(order => order.Items)
            .SingleOrDefaultAsync(order => order.Id == orderId, cancellationToken);
    }

    public async Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        _dbContext.Orders.Update(order);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return order;
    }

    public async Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(order => order.Items)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Order>> GetByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Orders
            .Include(order => order.Items)
            .Where(order => order.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }
}
