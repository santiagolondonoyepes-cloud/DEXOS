using System.Security.Claims;
using DEXOS.Application.Abstractions;

namespace DEXOS.API.Tenancy;

/// <summary>
/// Resuelve tenant/branch/user desde claims JWT o headers de compatibilidad.
/// </summary>
public sealed class HttpTenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpTenantContext(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? TenantId =>
        ParseGuid(FindClaim("tenant_id")) ??
        ParseGuid(_httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"].FirstOrDefault());

    public Guid? BranchId =>
        ParseGuid(FindClaim("branch_id")) ??
        ParseGuid(_httpContextAccessor.HttpContext?.Request.Headers["X-Branch-Id"].FirstOrDefault());

    public string? UserId => FindClaim(ClaimTypes.NameIdentifier) ?? FindClaim("sub");

    public IReadOnlyCollection<string> Roles =>
        _httpContextAccessor.HttpContext?.User?.Claims
            .Where(c => c.Type == ClaimTypes.Role || c.Type == "role")
            .Select(c => c.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray()
        ?? Array.Empty<string>();

    private string? FindClaim(string type)
    {
        return _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type == type)?.Value;
    }

    private static Guid? ParseGuid(string? value)
    {
        return Guid.TryParse(value, out var id) ? id : null;
    }
}
