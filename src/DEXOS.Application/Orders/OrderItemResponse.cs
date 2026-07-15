namespace DEXOS.Application.Orders;

public sealed record OrderItemResponse(Guid Id, Guid ProductId, string Description, int Quantity, decimal UnitPrice, decimal Total);
