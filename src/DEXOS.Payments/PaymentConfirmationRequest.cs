namespace DEXOS.Payments;

/// <summary>
/// Confirmación de pago enviada por un adapter o webhook interno.
/// </summary>
public sealed record PaymentConfirmationRequest(
    Guid OrderId,
    Guid TenantId,
    string PaymentReference,
    string Provider);
