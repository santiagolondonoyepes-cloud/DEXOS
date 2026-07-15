using DEXOS.Application.Abstractions;

namespace DEXOS.Application.Orders;

/// <summary>
/// Implementación por defecto no-op cuando no hay transporte realtime configurado.
/// </summary>
public sealed class NullRealtimeNotifier : IRealtimeNotifier
{
    public Task NotifyOrderPaidAsync(Guid tenantId, Guid branchId, Guid orderId, string orderNumber, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public Task NotifyKitchenOrderCreatedAsync(Guid tenantId, Guid branchId, Guid orderId, string station, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
