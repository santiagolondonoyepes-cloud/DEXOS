using DEXOS.Application.Orders;
using DEXOS.Domain.Entities;
using DEXOS.Domain.ValueObjects;
using Xunit;

namespace DEXOS.Domain.Tests;

public class OrderPaymentFlowTests
{
    [Fact]
    public void SendToKitchen_Throws_WhenOrderIsNotPaid()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), "ORD-2001");
        order.AddItem(new OrderItem(Guid.NewGuid(), "Burger", 1, new Money(12.00m)));
        order.Confirm();

        var service = new OrderService();

        var exception = Assert.Throws<InvalidOperationException>(() => service.SendToKitchen(order));

        Assert.Contains("pagada", exception.Message);
    }

    [Fact]
    public void SendToKitchen_AllowsTransition_WhenOrderIsPaid()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), "ORD-2002");
        order.AddItem(new OrderItem(Guid.NewGuid(), "Pizza", 1, new Money(15.00m)));
        order.Confirm();
        order.MarkPaid("pay_123");

        var service = new OrderService();
        var result = service.SendToKitchen(order);

        Assert.Equal(OrderStatus.SentToKitchen, result.Status);
    }
}
