namespace DEXOS.Payments;

/// <summary>
/// Contrato de integración para procesar pagos por tenant sin acoplar el dominio a una pasarela concreta.
/// </summary>
public interface IPaymentService
{
    Task<PaymentResult> CreatePaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default);
    Task<PaymentResult> ConfirmPaymentAsync(PaymentConfirmationRequest request, CancellationToken cancellationToken = default);
    Task<PaymentWebhookResult> HandleWebhookAsync(PaymentWebhookRequest request, CancellationToken cancellationToken = default);
}
