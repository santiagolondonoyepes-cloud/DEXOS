namespace DEXOS.Domain.ValueObjects;

public enum OrderStatus
{
    Created = 0,
    PendingPayment = 1,
    Paid = 2,
    SentToKitchen = 3,
    Preparing = 4,
    Ready = 5,
    Delivering = 6,
    Delivered = 7,
    Completed = 8,
    Cancelled = 9,
    Refunded = 10,
    Failed = 11
}
