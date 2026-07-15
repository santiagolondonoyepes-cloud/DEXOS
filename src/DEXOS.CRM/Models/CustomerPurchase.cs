namespace DEXOS.CRM.Models;

/// <summary>
/// Historial de compra de cliente para segmentacion y fidelizacion.
/// </summary>
public class CustomerPurchase
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TenantId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTimeOffset PurchasedAt { get; private set; } = DateTimeOffset.UtcNow;

    public CustomerPurchase(Guid tenantId, Guid customerId, Guid orderId, decimal amount)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId es obligatorio.", nameof(tenantId));
        if (customerId == Guid.Empty) throw new ArgumentException("CustomerId es obligatorio.", nameof(customerId));
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId es obligatorio.", nameof(orderId));
        if (amount <= 0m) throw new ArgumentOutOfRangeException(nameof(amount));

        TenantId = tenantId;
        CustomerId = customerId;
        OrderId = orderId;
        Amount = amount;
    }
}
