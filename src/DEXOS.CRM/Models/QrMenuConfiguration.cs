namespace DEXOS.CRM.Models;

/// <summary>
/// Configuracion centralizada del menu QR por tenant/sucursal.
/// </summary>
public class QrMenuConfiguration
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TenantId { get; private set; }
    public Guid? BranchId { get; private set; }
    public string Mode { get; private set; } = "internal"; // internal|external
    public string QrToken { get; private set; } = Guid.NewGuid().ToString("N");
    public string? InternalPath { get; private set; }
    public string? ExternalUrl { get; private set; }
    public string? WebhookSecret { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;

    public QrMenuConfiguration(Guid tenantId, Guid? branchId, string mode, string? internalPath, string? externalUrl, string? webhookSecret)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId es obligatorio.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(mode)) throw new ArgumentException("Mode es obligatorio.", nameof(mode));

        TenantId = tenantId;
        BranchId = branchId;
        Configure(mode, internalPath, externalUrl, webhookSecret);
    }

    public void Configure(string mode, string? internalPath, string? externalUrl, string? webhookSecret)
    {
        mode = mode.Trim().ToLowerInvariant();
        if (mode is not ("internal" or "external"))
        {
            throw new InvalidOperationException("Mode debe ser internal o external.");
        }

        if (mode == "internal" && string.IsNullOrWhiteSpace(internalPath))
        {
            throw new InvalidOperationException("InternalPath es requerido para modo internal.");
        }

        if (mode == "external" && string.IsNullOrWhiteSpace(externalUrl))
        {
            throw new InvalidOperationException("ExternalUrl es requerido para modo external.");
        }

        Mode = mode;
        InternalPath = internalPath?.Trim();
        ExternalUrl = externalUrl?.Trim();
        WebhookSecret = webhookSecret?.Trim();
        UpdatedAt = DateTimeOffset.UtcNow;
    }
}
