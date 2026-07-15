namespace DEXOS.Application.Orders;

public sealed record OrderResponse(
    Guid Id,
    Guid TenantId,
    Guid BranchId,
    string Number,
    string Status,
    decimal Total,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    IReadOnlyCollection<OrderItemResponse> Items);
