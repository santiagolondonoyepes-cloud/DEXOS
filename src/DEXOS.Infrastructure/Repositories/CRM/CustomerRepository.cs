using DEXOS.CRM.Abstractions;
using DEXOS.CRM.Models;
using DEXOS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DEXOS.Infrastructure.Repositories.CRM;

/// <summary>
/// Repositorio EF Core para clientes.
/// </summary>
public sealed class CustomerRepository : ICustomerRepository
{
    private readonly DexosDbContext _dbContext;

    public CustomerRepository(DexosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(customer);
        await _dbContext.Customers.AddAsync(customer, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return customer;
    }

    public async Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers.SingleOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Customer?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .SingleOrDefaultAsync(c => c.TenantId == tenantId && c.Email == email, cancellationToken);
    }

    public async Task<IReadOnlyCollection<Customer>> GetByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Customers
            .Where(c => c.TenantId == tenantId)
            .OrderBy(c => c.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(customer);
        _dbContext.Customers.Update(customer);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddPurchaseAsync(CustomerPurchase purchase, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(purchase);
        await _dbContext.CustomerPurchases.AddAsync(purchase, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<CustomerPurchase>> GetPurchasesByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.CustomerPurchases
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.PurchasedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddCouponAsync(LoyaltyCoupon coupon, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(coupon);
        await _dbContext.LoyaltyCoupons.AddAsync(coupon, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<LoyaltyCoupon>> GetCouponsByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.LoyaltyCoupons
            .Where(x => x.CustomerId == customerId)
            .OrderByDescending(x => x.ExpiresAt)
            .ToListAsync(cancellationToken);
    }
}
