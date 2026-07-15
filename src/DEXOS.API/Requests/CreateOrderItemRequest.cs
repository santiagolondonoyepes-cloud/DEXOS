namespace DEXOS.API.Requests;

public sealed record CreateOrderItemRequest(Guid ProductId, string Description, int Quantity, decimal UnitPrice);
