using DEXOS.Application.Orders;
using DEXOS.Domain.ValueObjects;
using DEXOS.Infrastructure.Repositories;
using Xunit;

namespace DEXOS.Domain.Tests;

public class OrderApplicationTests
{
    [Fact]
    public async Task CreateOrder_StoresOrderAndReturnsReference()
    {
        var repository = new InMemoryOrderRepository();
        var useCase = new CreateOrderUseCase(repository);

        var result = await useCase.ExecuteAsync(new CreateOrderCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "ORD-1001",
            null,
            Array.Empty<CreateOrderItemCommand>()));

        Assert.NotNull(result);
        Assert.Equal("ORD-1001", result.Number);
        Assert.Equal(OrderStatus.Created, result.Status);
    }
}
