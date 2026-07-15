using DEXOS.Domain.Entities;
using DEXOS.Domain.ValueObjects;

namespace DEXOS.Application.Orders;

/// <summary>
/// Servicio de aplicación encargado de orquestar el ciclo de vida de una Orden.
/// </summary>
public class OrderService
{
    public Order CreateOrder(Guid tenantId, Guid branchId, string number)
    {
        return new Order(tenantId, branchId, number);
    }

    public Order AddItem(Order order, Guid productId, string description, int quantity, Money unitPrice)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.AddItem(new OrderItem(productId, description, quantity, unitPrice));
        return order;
    }

    public Order Confirm(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.Confirm();
        return order;
    }

    public Order MarkPaid(Order order, string paymentReference)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.MarkPaid(paymentReference);
        return order;
    }

    public Order SendToKitchen(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.SendToKitchen();
        return order;
    }

    public Order StartPreparation(Order order)
    {
        ArgumentNullException.ThrowIfNull(order);
        order.StartPreparation();
        return order;
    }
}
