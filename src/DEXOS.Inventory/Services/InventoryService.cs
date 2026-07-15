using DEXOS.Inventory.Abstractions;
using DEXOS.Inventory.Models;

namespace DEXOS.Inventory.Services;

/// <summary>
/// Servicio de aplicación para orquestar operaciones de inventario.
/// </summary>
public class InventoryService
{
    private readonly IInventoryRepository _repository;

    public InventoryService(IInventoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Product> CreateProductAsync(Guid tenantId, string sku, string name, string description, decimal price, string unitOfMeasure, int stock, int minStock, int maxStock, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetProductBySkuAsync(tenantId, sku, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException($"Ya existe un producto con SKU '{sku}'.");
        }

        var product = new Product(tenantId, sku, name, description, price, unitOfMeasure, stock, minStock, maxStock);
        return await _repository.AddProductAsync(product, cancellationToken);
    }

    public async Task<Product> AdjustStockAsync(Guid productId, int delta, string reason, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetProductByIdAsync(productId, cancellationToken)
            ?? throw new KeyNotFoundException("Producto no encontrado.");

        product.AdjustStock(delta);
        await _repository.UpdateProductAsync(product, cancellationToken);
        await _repository.AddMovementAsync(new InventoryMovement(product.Id, product.TenantId, delta > 0 ? "PURCHASE" : "SALE", Math.Abs(delta), reason), cancellationToken);

        return product;
    }

    public async Task<IReadOnlyCollection<Product>> GetProductsAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetProductsAsync(tenantId, cancellationToken);
    }
}
