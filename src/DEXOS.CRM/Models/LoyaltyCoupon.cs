namespace DEXOS.CRM.Models;

/// <summary>
/// Cupon emitido a clientes por programa de fidelizacion.
/// </summary>
public class LoyaltyCoupon
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TenantId { get; private set; }
    public Guid CustomerId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public decimal DiscountPercentage { get; private set; }
    public DateTimeOffset ExpiresAt { get; private set; }
    public bool IsRedeemed { get; private set; }

    public LoyaltyCoupon(Guid tenantId, Guid customerId, string code, decimal discountPercentage, DateTimeOffset expiresAt)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId es obligatorio.", nameof(tenantId));
        if (customerId == Guid.Empty) throw new ArgumentException("CustomerId es obligatorio.", nameof(customerId));
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Code es obligatorio.", nameof(code));
        if (discountPercentage <= 0 || discountPercentage > 100) throw new ArgumentOutOfRangeException(nameof(discountPercentage));

        TenantId = tenantId;
        CustomerId = customerId;
        Code = code.Trim().ToUpperInvariant();
        DiscountPercentage = discountPercentage;
        ExpiresAt = expiresAt;
    }

    public void Redeem()
    {
        if (DateTimeOffset.UtcNow > ExpiresAt)
        {
            throw new InvalidOperationException("El cupon esta expirado.");
        }

        if (IsRedeemed)
        {
            throw new InvalidOperationException("El cupon ya fue redimido.");
        }

        IsRedeemed = true;
    }
}
