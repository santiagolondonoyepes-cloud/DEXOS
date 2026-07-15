using DEXOS.CRM.Models;

namespace DEXOS.CRM.Abstractions;

public interface IQrMenuRepository
{
    Task<QrMenuConfiguration?> GetByTenantAsync(Guid tenantId, Guid? branchId, CancellationToken cancellationToken = default);
    Task<QrMenuConfiguration?> GetByTokenAsync(string qrToken, CancellationToken cancellationToken = default);
    Task<QrMenuConfiguration> AddAsync(QrMenuConfiguration configuration, CancellationToken cancellationToken = default);
    Task UpdateAsync(QrMenuConfiguration configuration, CancellationToken cancellationToken = default);
}
