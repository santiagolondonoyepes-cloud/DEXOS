namespace DEXOS.Orders.Models;

/// <summary>
/// Comanda digital creada a partir de una orden pagada.
/// </summary>
public class KitchenOrder
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid OrderId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Station { get; private set; } = "kitchen";
    public string Status { get; private set; } = "PREPARING";
    public string Notes { get; private set; } = string.Empty;

    public KitchenOrder(Guid orderId, Guid tenantId, string station, string? notes = null)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId es obligatorio.", nameof(orderId));
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId es obligatorio.", nameof(tenantId));
        OrderId = orderId;
        TenantId = tenantId;
        Station = string.IsNullOrWhiteSpace(station) ? "kitchen" : station.Trim().ToLowerInvariant();
        Notes = notes?.Trim() ?? string.Empty;
    }

    public void MarkReady() => Status = "READY";
    public void MarkDelivered() => Status = "DELIVERED";
}
