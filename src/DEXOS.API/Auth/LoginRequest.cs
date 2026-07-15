namespace DEXOS.API.Auth;

public sealed class LoginRequest
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public Guid TenantId { get; init; }
    public Guid? BranchId { get; init; }
}
