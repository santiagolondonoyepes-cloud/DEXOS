using DEXOS.API.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DEXOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IOptions<AuthOptions> _authOptions;
    private readonly JwtTokenService _jwtTokenService;

    public AuthController(IOptions<AuthOptions> authOptions, JwtTokenService jwtTokenService)
    {
        _authOptions = authOptions;
        _jwtTokenService = jwtTokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var user = _authOptions.Value.Users.FirstOrDefault(u =>
            u.Username.Equals(request.Username, StringComparison.OrdinalIgnoreCase) &&
            u.Password == request.Password &&
            u.TenantId == request.TenantId &&
            (request.BranchId == null || u.BranchId == request.BranchId));

        if (user is null)
        {
            return Unauthorized(new { message = "Credenciales invalidas o tenant no autorizado." });
        }

        var token = _jwtTokenService.CreateToken(user);
        return Ok(token);
    }
}
