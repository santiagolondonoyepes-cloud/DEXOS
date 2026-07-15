namespace DEXOS.API.Requests;

public sealed class PaymentWebhookRequest
{
    public Guid TenantId { get; init; }
    public string Provider { get; init; } = "simulated";
    public string EventType { get; init; } = "payment.succeeded";
    public string PaymentReference { get; init; } = string.Empty;
    public string? RawPayload { get; init; }
}
