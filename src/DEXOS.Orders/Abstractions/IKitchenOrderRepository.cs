using DEXOS.Orders.Models;

namespace DEXOS.Orders.Abstractions;

public interface IKitchenOrderRepository
{
    Task<KitchenOrder> AddAsync(KitchenOrder order, CancellationToken cancellationToken = default);
    Task<KitchenOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<KitchenOrder>> GetByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task UpdateAsync(KitchenOrder order, CancellationToken cancellationToken = default);
}
