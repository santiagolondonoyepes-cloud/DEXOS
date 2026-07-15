using DEXOS.Application.Abstractions;
using DEXOS.Domain.ValueObjects;

namespace DEXOS.Application.Orders;

public sealed class AddOrderItemUseCase
{
    private readonly IOrderRepository _orderRepository;

    public AddOrderItemUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<OrderResponse> ExecuteAsync(AddOrderItemCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Orden no encontrada.");

        order.AddItem(new Domain.Entities.OrderItem(command.ProductId, command.Description, command.Quantity, Money.FromDecimal(command.UnitPrice)));
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order.ToResponse();
    }
}
