namespace DEXOS.Application.Abstractions;

/// <summary>
/// Abstracción para publicar notificaciones operativas en tiempo real.
/// </summary>
public interface IRealtimeNotifier
{
    Task NotifyOrderPaidAsync(Guid tenantId, Guid branchId, Guid orderId, string orderNumber, CancellationToken cancellationToken = default);
    Task NotifyKitchenOrderCreatedAsync(Guid tenantId, Guid branchId, Guid orderId, string station, CancellationToken cancellationToken = default);
}
