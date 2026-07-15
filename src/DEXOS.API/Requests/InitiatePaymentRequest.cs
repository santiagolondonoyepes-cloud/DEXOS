namespace DEXOS.API.Requests;

public sealed class InitiatePaymentRequest
{
    public Guid TenantId { get; init; }
    public string Provider { get; init; } = "simulated";
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "EUR";
    public string? Description { get; init; }
    public string? ReturnUrl { get; init; }
}
