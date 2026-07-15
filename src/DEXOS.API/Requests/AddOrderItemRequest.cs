namespace DEXOS.API.Requests;

public sealed record AddOrderItemRequest(Guid ProductId, string Description, int Quantity, decimal UnitPrice);
