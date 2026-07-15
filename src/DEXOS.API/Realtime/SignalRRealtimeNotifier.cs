using DEXOS.Application.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace DEXOS.API.Realtime;

/// <summary>
/// Implementación realtime sobre SignalR.
/// </summary>
public sealed class SignalRRealtimeNotifier : IRealtimeNotifier
{
    private readonly IHubContext<OperationsHub> _hubContext;

    public SignalRRealtimeNotifier(IHubContext<OperationsHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyOrderPaidAsync(Guid tenantId, Guid branchId, Guid orderId, string orderNumber, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .Group(OperationsHub.GroupName(tenantId, branchId))
            .SendAsync("OrderPaid", new
            {
                TenantId = tenantId,
                BranchId = branchId,
                OrderId = orderId,
                OrderNumber = orderNumber,
                OccurredAt = DateTimeOffset.UtcNow
            }, cancellationToken);
    }

    public async Task NotifyKitchenOrderCreatedAsync(Guid tenantId, Guid branchId, Guid orderId, string station, CancellationToken cancellationToken = default)
    {
        await _hubContext.Clients
            .Group(OperationsHub.GroupName(tenantId, branchId))
            .SendAsync("KitchenOrderCreated", new
            {
                TenantId = tenantId,
                BranchId = branchId,
                OrderId = orderId,
                Station = station,
                OccurredAt = DateTimeOffset.UtcNow
            }, cancellationToken);
    }
}
