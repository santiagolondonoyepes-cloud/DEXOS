using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DEXOS.API.Realtime;

/// <summary>
/// Hub para notificaciones operativas por tenant y sucursal.
/// </summary>
[Authorize]
public sealed class OperationsHub : Hub
{
    public Task JoinTenantBranch(Guid tenantId, Guid branchId)
    {
        return Groups.AddToGroupAsync(Context.ConnectionId, GroupName(tenantId, branchId));
    }

    public Task LeaveTenantBranch(Guid tenantId, Guid branchId)
    {
        return Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(tenantId, branchId));
    }

    public static string GroupName(Guid tenantId, Guid branchId) => $"tenant:{tenantId}:branch:{branchId}";
}
