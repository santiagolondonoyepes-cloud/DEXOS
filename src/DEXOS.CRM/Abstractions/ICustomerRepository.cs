using DEXOS.CRM.Models;

namespace DEXOS.CRM.Abstractions;

public interface ICustomerRepository
{
    Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Customer?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<Customer>> GetByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default);

    Task AddPurchaseAsync(CustomerPurchase purchase, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CustomerPurchase>> GetPurchasesByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);

    Task AddCouponAsync(LoyaltyCoupon coupon, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<LoyaltyCoupon>> GetCouponsByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default);
}
