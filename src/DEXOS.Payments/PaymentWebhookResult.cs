namespace DEXOS.Payments;

/// <summary>
/// Resultado neutral de un webhook de pago.
/// </summary>
public sealed record PaymentWebhookResult(
    bool IsSuccess,
    string Status,
    string PaymentReference,
    string? Message = null);
