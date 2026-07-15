using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DEXOS.API.Auth;

public sealed class JwtTokenService
{
    private readonly IConfiguration _configuration;

    public JwtTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public LoginResponse CreateToken(AuthUser user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var issuer = jwtSection["Issuer"] ?? "DEXOS";
        var audience = jwtSection["Audience"] ?? "DEXOS.Clients";
        var secret = jwtSection["Secret"] ?? "DEXOS_DEV_CHANGE_THIS_SECRET_32_CHARS_MIN";
        var expiresMinutes = int.TryParse(jwtSection["AccessTokenMinutes"], out var minutes) ? minutes : 120;

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Username),
            new(ClaimTypes.NameIdentifier, user.Username),
            new(ClaimTypes.Name, user.Username),
            new("tenant_id", user.TenantId.ToString()),
            new("branch_id", user.BranchId?.ToString() ?? string.Empty)
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var expires = DateTimeOffset.UtcNow.AddMinutes(expiresMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expires.UtcDateTime,
            signingCredentials: credentials);

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new LoginResponse
        {
            AccessToken = accessToken,
            ExpiresAtUtc = expires,
            Username = user.Username,
            TenantId = user.TenantId,
            BranchId = user.BranchId,
            Roles = user.Roles
        };
    }
}
