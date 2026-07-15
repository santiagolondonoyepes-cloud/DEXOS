namespace DEXOS.Inventory.Models;

/// <summary>
/// Movimiento de kardex para trazabilidad de inventario.
/// </summary>
public class InventoryMovement
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Type { get; private set; } = string.Empty;
    public int Quantity { get; private set; }
    public string Reason { get; private set; } = string.Empty;
    public DateTimeOffset OccurredAt { get; private set; } = DateTimeOffset.UtcNow;

    public InventoryMovement(Guid productId, Guid tenantId, string type, int quantity, string reason)
    {
        if (productId == Guid.Empty) throw new ArgumentException("ProductId es obligatorio.", nameof(productId));
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId es obligatorio.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(type)) throw new ArgumentException("Tipo es obligatorio.", nameof(type));
        if (quantity == 0) throw new ArgumentOutOfRangeException(nameof(quantity));

        ProductId = productId;
        TenantId = tenantId;
        Type = type.Trim();
        Quantity = quantity;
        Reason = reason.Trim();
    }
}
