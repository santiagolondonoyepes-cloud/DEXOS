namespace DEXOS.Inventory.Models;

/// <summary>
/// Producto del catálogo de inventario con SKU único y stock configurable.
/// </summary>
public class Product
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TenantId { get; private set; }
    public string Sku { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public string UnitOfMeasure { get; private set; } = "unit";
    public int Stock { get; private set; }
    public int MinStock { get; private set; }
    public int MaxStock { get; private set; }
    public bool IsActive { get; private set; } = true;

    public Product(Guid tenantId, string sku, string name, string description, decimal price, string unitOfMeasure, int stock, int minStock, int maxStock)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId es obligatorio.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(sku)) throw new ArgumentException("SKU es obligatorio.", nameof(sku));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Nombre es obligatorio.", nameof(name));
        if (price < 0) throw new ArgumentOutOfRangeException(nameof(price));
        if (stock < 0) throw new ArgumentOutOfRangeException(nameof(stock));
        if (minStock < 0 || maxStock < minStock) throw new ArgumentOutOfRangeException(nameof(maxStock));

        TenantId = tenantId;
        Sku = sku.Trim().ToUpperInvariant();
        Name = name.Trim();
        Description = description.Trim();
        Price = price;
        UnitOfMeasure = string.IsNullOrWhiteSpace(unitOfMeasure) ? "unit" : unitOfMeasure.Trim();
        Stock = stock;
        MinStock = minStock;
        MaxStock = maxStock;
    }

    public void AdjustStock(int delta)
    {
        var next = Stock + delta;
        if (next < 0) throw new InvalidOperationException("No hay suficiente stock para ejecutar esta operación.");
        Stock = next;
    }

    public bool RequiresAlert() => Stock <= MinStock;
}
