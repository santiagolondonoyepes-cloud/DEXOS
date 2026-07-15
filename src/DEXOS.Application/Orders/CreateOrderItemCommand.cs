namespace DEXOS.Application.Orders;

public sealed record CreateOrderItemCommand(Guid ProductId, string Description, int Quantity, decimal UnitPrice);
