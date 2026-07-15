namespace DEXOS.Application.Orders;

public sealed record InitiatePaymentCommand(
    Guid OrderId,
    Guid TenantId,
    string Provider,
    decimal Amount,
    string Currency = "EUR",
    string? Description = null,
    string? ReturnUrl = null);
