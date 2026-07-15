namespace DEXOS.Payments;

/// <summary>
/// Adapter stub para simular el procesamiento de pagos sin depender de Stripe o Checkout.com.
/// </summary>
public sealed class SimulatedPaymentService : IPaymentService
{
    public Task<PaymentResult> CreatePaymentAsync(PaymentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var paymentReference = $"sim_{Guid.NewGuid():N}";
        var checkoutUrl = $"https://payments.dexos.test/checkout/{paymentReference}";

        var result = new PaymentResult(
            IsSuccess: true,
            Status: "Pending",
            PaymentReference: paymentReference,
            Provider: request.Provider,
            Message: "Pago simulado creado correctamente.",
            CheckoutUrl: checkoutUrl);

        return Task.FromResult(result);
    }

    public Task<PaymentResult> ConfirmPaymentAsync(PaymentConfirmationRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (string.IsNullOrWhiteSpace(request.PaymentReference))
        {
            return Task.FromResult(new PaymentResult(false, "Failed", string.Empty, request.Provider, "Referencia de pago requerida."));
        }

        var result = new PaymentResult(
            IsSuccess: true,
            Status: "Succeeded",
            PaymentReference: request.PaymentReference,
            Provider: request.Provider,
            Message: "Pago confirmado de forma simulada.");

        return Task.FromResult(result);
    }

    public Task<PaymentWebhookResult> HandleWebhookAsync(PaymentWebhookRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var status = request.EventType.Equals("payment.succeeded", StringComparison.OrdinalIgnoreCase)
            ? "Succeeded"
            : "Received";

        var result = new PaymentWebhookResult(
            IsSuccess: true,
            Status: status,
            PaymentReference: request.PaymentReference,
            Message: $"Webhook simulado recibido para {request.Provider}.");

        return Task.FromResult(result);
    }
}
