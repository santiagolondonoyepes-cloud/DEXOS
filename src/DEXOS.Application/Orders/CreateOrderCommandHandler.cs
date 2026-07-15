using DEXOS.Application.Abstractions;
using DEXOS.Domain.Entities;

namespace DEXOS.Application.Orders;

/// <summary>
/// Caso de uso de aplicación para crear una nueva orden dentro del contexto de un tenant y una sucursal.
/// </summary>
public class CreateOrderCommandHandler(IOrderRepository orderRepository)
{
    public async Task<Order> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(command);

        var order = new Order(command.TenantId, command.BranchId, command.Number, command.CustomerId);
        await orderRepository.AddAsync(order, cancellationToken);
        return order;
    }
}
