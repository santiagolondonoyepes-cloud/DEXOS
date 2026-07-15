namespace DEXOS.API.Auth;

public sealed class AuthUser
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public Guid TenantId { get; init; }
    public Guid? BranchId { get; init; }
    public string[] Roles { get; init; } = Array.Empty<string>();
}
