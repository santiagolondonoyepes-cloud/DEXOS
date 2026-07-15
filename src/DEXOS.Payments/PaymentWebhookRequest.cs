namespace DEXOS.Payments;

/// <summary>
/// Modelo neutral para un webhook de una pasarela de pagos.
/// </summary>
public sealed record PaymentWebhookRequest(
    Guid OrderId,
    Guid TenantId,
    string Provider,
    string EventType,
    string PaymentReference,
    string? RawPayload = null);
