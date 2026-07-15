namespace DEXOS.Application.Orders;

public sealed record CreateOrderCommand(Guid TenantId, Guid BranchId, string Number, Guid? CustomerId, IReadOnlyCollection<CreateOrderItemCommand> Items);
