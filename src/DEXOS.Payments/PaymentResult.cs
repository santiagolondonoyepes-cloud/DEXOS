namespace DEXOS.Payments;

/// <summary>
/// Resultado del intento de pago o confirmación.
/// </summary>
public sealed record PaymentResult(
    bool IsSuccess,
    string Status,
    string PaymentReference,
    string? Provider = null,
    string? Message = null,
    string? CheckoutUrl = null);
