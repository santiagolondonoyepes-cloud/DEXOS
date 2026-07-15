using DEXOS.Domain.Entities;

namespace DEXOS.Application.Abstractions;

public interface IOrderRepository
{
    Task<Order> AddAsync(Order order, CancellationToken cancellationToken = default);
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<Order> UpdateAsync(Order order, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Order>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Order>> GetByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
