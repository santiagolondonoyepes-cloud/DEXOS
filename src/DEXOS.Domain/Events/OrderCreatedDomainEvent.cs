using DEXOS.Domain.Common;

namespace DEXOS.Domain.Events;

public sealed class OrderCreatedDomainEvent : IDomainEvent
{
    public Guid AggregateId { get; }
    public Guid TenantId { get; }
    public Guid BranchId { get; }
    public string OrderNumber { get; }
    public DateTimeOffset OccurredAt { get; }

    public OrderCreatedDomainEvent(Guid aggregateId, Guid tenantId, Guid branchId, string orderNumber)
    {
        AggregateId = aggregateId;
        TenantId = tenantId;
        BranchId = branchId;
        OrderNumber = orderNumber;
        OccurredAt = DateTimeOffset.UtcNow;
    }
}

