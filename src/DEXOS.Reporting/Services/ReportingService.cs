namespace DEXOS.Reporting.Services;

/// <summary>
/// Servicio base para reportes y KPIs del negocio.
/// </summary>
public class ReportingService
{
    public IReadOnlyCollection<string> GetSalesMetrics() => new[] { "ventas-hoy", "ventas-mes", "ticket-promedio" };
    public IReadOnlyCollection<string> GetInventoryMetrics() => new[] { "stock-bajo", "productos-activos", "rotacion" };
    public IReadOnlyCollection<string> GetCustomerMetrics() => new[] { "clientes-nuevos", "puntos-acumulados", "segmentos" };
}
