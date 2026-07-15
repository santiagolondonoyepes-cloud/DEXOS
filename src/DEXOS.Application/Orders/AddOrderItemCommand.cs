namespace DEXOS.Application.Orders;

public sealed record AddOrderItemCommand(Guid OrderId, Guid ProductId, string Description, int Quantity, decimal UnitPrice);
