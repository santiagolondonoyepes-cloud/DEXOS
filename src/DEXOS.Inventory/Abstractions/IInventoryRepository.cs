using DEXOS.Inventory.Models;

namespace DEXOS.Inventory.Abstractions;

public interface IInventoryRepository
{
    Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<Product?> GetProductBySkuAsync(Guid tenantId, string sku, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Product>> GetProductsAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
    Task AddMovementAsync(InventoryMovement movement, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<InventoryMovement>> GetMovementsAsync(Guid productId, CancellationToken cancellationToken = default);
}
