using DEXOS.Application.Abstractions;
using DEXOS.Domain.Entities;

namespace DEXOS.Application.Orders;

public sealed class GetOrderByIdUseCase
{
    private readonly IOrderRepository _orderRepository;

    public GetOrderByIdUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> ExecuteAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken);
        return order ?? throw new KeyNotFoundException("Orden no encontrada.");
    }
}
