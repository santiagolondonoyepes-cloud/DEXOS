using DEXOS.Domain.Common;

namespace DEXOS.Domain.Events;

public sealed class OrderPaymentPendingDomainEvent : IDomainEvent
{
    public Guid AggregateId { get; }
    public Guid TenantId { get; }
    public Guid BranchId { get; }
    public string OrderNumber { get; }
    public DateTimeOffset OccurredAt { get; }

    public OrderPaymentPendingDomainEvent(Guid aggregateId, Guid tenantId, Guid branchId, string orderNumber)
    {
        AggregateId = aggregateId;
        TenantId = tenantId;
        BranchId = branchId;
        OrderNumber = orderNumber;
        OccurredAt = DateTimeOffset.UtcNow;
    }
}
