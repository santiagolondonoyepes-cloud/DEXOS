namespace DEXOS.Payments;

/// <summary>
/// Solicitud de pago generada por un tenant para una orden concreta.
/// </summary>
public sealed record PaymentRequest(
    Guid OrderId,
    Guid TenantId,
    decimal Amount,
    string Currency,
    string Provider,
    string? Description = null,
    string? ReturnUrl = null);
