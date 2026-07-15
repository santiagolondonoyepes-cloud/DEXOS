using DEXOS.Application.Abstractions;
using DEXOS.Domain.Entities;
using DEXOS.Domain.ValueObjects;

namespace DEXOS.Application.Orders;

public sealed class CreateOrderUseCase
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderUseCase(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> ExecuteAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var order = new Order(command.TenantId, command.BranchId, command.Number, command.CustomerId);

        foreach (var itemCommand in command.Items ?? Array.Empty<CreateOrderItemCommand>())
        {
            order.AddItem(new OrderItem(
                itemCommand.ProductId,
                itemCommand.Description,
                itemCommand.Quantity,
                Money.FromDecimal(itemCommand.UnitPrice)));
        }

        await _orderRepository.AddAsync(order, cancellationToken);
        return order;
    }
}
