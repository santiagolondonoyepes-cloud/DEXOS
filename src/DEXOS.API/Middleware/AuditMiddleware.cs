using System.Diagnostics;

namespace DEXOS.API.Middleware;

/// <summary>
/// Middleware de auditoria para endpoints criticos y trazabilidad de transacciones.
/// </summary>
public sealed class AuditMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuditMiddleware> _logger;

    public AuditMiddleware(RequestDelegate next, ILogger<AuditMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        var correlationId = context.TraceIdentifier;
        var sw = Stopwatch.StartNew();

        await _next(context);

        sw.Stop();

        if (IsCriticalPath(context.Request.Path))
        {
            _logger.LogInformation(
                "AUDIT CorrelationId={CorrelationId} Method={Method} Path={Path} StatusCode={StatusCode} Tenant={TenantId} Branch={BranchId} DurationMs={DurationMs}",
                correlationId,
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                context.Request.Headers["X-Tenant-Id"].ToString(),
                context.Request.Headers["X-Branch-Id"].ToString(),
                sw.ElapsedMilliseconds);
        }
    }

    private static bool IsCriticalPath(PathString path)
    {
        var value = path.Value?.ToLowerInvariant() ?? string.Empty;
        return value.Contains("/payments") || value.Contains("/send-to-kitchen") || value.Contains("/orders");
    }
}
