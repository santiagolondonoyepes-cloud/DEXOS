using DEXOS.Application.Abstractions;

namespace DEXOS.Application.Orders;

/// <summary>
/// Caso de uso para enviar una orden a cocina cuando ya ha sido pagada.
/// </summary>
public sealed class SendOrderToKitchenUseCase
{
    private readonly IOrderRepository _orderRepository;

    public SendOrderToKitchenUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> ExecuteAsync(SendOrderToKitchenCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Orden no encontrada.");

        if (order.TenantId != command.TenantId)
        {
            throw new InvalidOperationException("La orden no pertenece al tenant indicado.");
        }

        order.SendToKitchen();
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order.ToResponse();
    }
}
