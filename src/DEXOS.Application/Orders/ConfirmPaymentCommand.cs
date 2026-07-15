namespace DEXOS.Application.Orders;

public sealed record ConfirmPaymentCommand(
    Guid OrderId,
    Guid TenantId,
    string Provider,
    string PaymentReference,
    string? EventType = null);
