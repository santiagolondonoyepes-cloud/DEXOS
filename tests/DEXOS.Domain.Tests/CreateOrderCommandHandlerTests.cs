using DEXOS.Application.Orders;
using DEXOS.Domain.Entities;
using DEXOS.Domain.ValueObjects;
using DEXOS.Infrastructure.Repositories;
using Xunit;

namespace DEXOS.Domain.Tests;

public class CreateOrderCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatesOrderAndPersistsIt()
    {
        var repository = new InMemoryOrderRepository();
        var handler = new CreateOrderCommandHandler(repository);
        var command = new CreateOrderCommand(Guid.NewGuid(), Guid.NewGuid(), "ORD-1001", null, Array.Empty<CreateOrderItemCommand>());

        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("ORD-1001", result.Number);
        Assert.Equal(OrderStatus.Created, result.Status);
        Assert.Single(await repository.GetAllAsync(CancellationToken.None));
    }
}
