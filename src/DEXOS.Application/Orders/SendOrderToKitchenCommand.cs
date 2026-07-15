namespace DEXOS.Application.Orders;

public sealed record SendOrderToKitchenCommand(Guid OrderId, Guid TenantId);
