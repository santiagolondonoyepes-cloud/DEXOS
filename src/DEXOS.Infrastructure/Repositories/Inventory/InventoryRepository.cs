using DEXOS.Infrastructure.Persistence;
using DEXOS.Inventory.Abstractions;
using DEXOS.Inventory.Models;
using Microsoft.EntityFrameworkCore;

namespace DEXOS.Infrastructure.Repositories.Inventory;

/// <summary>
/// Repositorio EF Core para gestión de productos e inventario.
/// </summary>
public sealed class InventoryRepository : IInventoryRepository
{
    private readonly DexosDbContext _dbContext;

    public InventoryRepository(DexosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        await _dbContext.Products.AddAsync(product, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return product;
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products.SingleOrDefaultAsync(p => p.Id == productId, cancellationToken);
    }

    public async Task<Product?> GetProductBySkuAsync(Guid tenantId, string sku, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .SingleOrDefaultAsync(p => p.TenantId == tenantId && p.Sku == sku.ToUpperInvariant(), cancellationToken);
    }

    public async Task<IReadOnlyCollection<Product>> GetProductsAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Products
            .Where(p => p.TenantId == tenantId && p.IsActive)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(product);
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddMovementAsync(InventoryMovement movement, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(movement);
        await _dbContext.InventoryMovements.AddAsync(movement, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<InventoryMovement>> GetMovementsAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.InventoryMovements
            .Where(m => m.ProductId == productId)
            .OrderByDescending(m => m.OccurredAt)
            .ToListAsync(cancellationToken);
    }
}
