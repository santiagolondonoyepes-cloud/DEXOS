using DEXOS.Domain.Common;

namespace DEXOS.Domain.Events;

public sealed class OrderPaidDomainEvent : IDomainEvent
{
    public Guid AggregateId { get; }
    public Guid TenantId { get; }
    public Guid BranchId { get; }
    public string OrderNumber { get; }
    public string PaymentReference { get; }
    public DateTimeOffset OccurredAt { get; }

    public OrderPaidDomainEvent(Guid aggregateId, Guid tenantId, Guid branchId, string orderNumber, string paymentReference)
    {
        AggregateId = aggregateId;
        TenantId = tenantId;
        BranchId = branchId;
        OrderNumber = orderNumber;
        PaymentReference = paymentReference;
        OccurredAt = DateTimeOffset.UtcNow;
    }
}
