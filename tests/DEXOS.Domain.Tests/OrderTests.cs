using DEXOS.Domain.Entities;
using DEXOS.Domain.ValueObjects;
using Xunit;

namespace DEXOS.Domain.Tests;

public class OrderTests
{
    [Fact]
    public void AddItem_UpdatesTotalAndStatusRemainsDraft()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), "ORD-001");

        order.AddItem(new OrderItem(Guid.NewGuid(), "Coffee", 2, new Money(3.50m)));

        Assert.Equal(OrderStatus.Created, order.Status);
        Assert.Equal(7.00m, order.Total.Amount);
    }

    [Fact]
    public void Confirm_WithItems_ChangesStatusToConfirmed()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), "ORD-002");
        order.AddItem(new OrderItem(Guid.NewGuid(), "Burger", 1, new Money(12.00m)));

        order.Confirm();

        Assert.Equal(OrderStatus.PendingPayment, order.Status);
    }

    [Fact]
    public void Confirm_WithoutItems_Throws()
    {
        var order = new Order(Guid.NewGuid(), Guid.NewGuid(), "ORD-003");

        var exception = Assert.Throws<InvalidOperationException>(() => order.Confirm());

        Assert.Contains("al menos un ítem", exception.Message);
    }
}
