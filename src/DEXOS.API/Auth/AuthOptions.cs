namespace DEXOS.API.Auth;

public sealed class AuthOptions
{
    public List<AuthUser> Users { get; init; } = new();
}
