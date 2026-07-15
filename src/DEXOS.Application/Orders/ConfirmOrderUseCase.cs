using DEXOS.Application.Abstractions;

namespace DEXOS.Application.Orders;

public sealed class ConfirmOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public ConfirmOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> ExecuteAsync(ConfirmOrderCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Orden no encontrada.");

        order.Confirm();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order.ToResponse();
    }
}
