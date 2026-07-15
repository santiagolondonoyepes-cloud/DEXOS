using DEXOS.Application.Abstractions;
using DEXOS.Domain.Entities;

namespace DEXOS.Application.Orders;

public sealed class GetOrdersUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetOrdersUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public Task<IReadOnlyCollection<Order>> ExecuteAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return _orderRepository.GetByTenantAsync(tenantId, cancellationToken);
    }
}
