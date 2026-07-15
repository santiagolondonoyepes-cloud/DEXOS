namespace DEXOS.Application.Abstractions;

/// <summary>
/// Contexto de tenant resuelto por request para aislamiento de datos y autorizacion.
/// </summary>
public interface ITenantContext
{
    Guid? TenantId { get; }
    Guid? BranchId { get; }
    string? UserId { get; }
    IReadOnlyCollection<string> Roles { get; }
}
