using DEXOS.API.Requests;

namespace DEXOS.API.Requests;

public sealed record CreateOrderRequest(Guid TenantId, Guid BranchId, string Number, Guid? CustomerId, IReadOnlyCollection<CreateOrderItemRequest> Items);
