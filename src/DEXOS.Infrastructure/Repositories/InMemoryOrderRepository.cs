using System.Collections.Concurrent;
using DEXOS.Application.Abstractions;
using DEXOS.Domain.Entities;

namespace DEXOS.Infrastructure.Repositories;

public sealed class InMemoryOrderRepository : IOrderRepository
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        _orders[order.Id] = order;
        return Task.FromResult(order);
    }

    public Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        _orders.TryGetValue(orderId, out var order);
        return Task.FromResult(order);
    }

    public Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(order);

        if (!_orders.ContainsKey(order.Id))
        {
            throw new KeyNotFoundException("La orden no existe en el repositorio.");
        }

        _orders[order.Id] = order;
        return Task.FromResult(order);
    }

    public Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<IReadOnlyCollection<Order>>(_orders.Values.ToList());
    }

    public Task<IReadOnlyCollection<Order>> GetByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var orders = _orders.Values.Where(order => order.TenantId == tenantId).ToList();
        return Task.FromResult<IReadOnlyCollection<Order>>(orders);
    }
}
