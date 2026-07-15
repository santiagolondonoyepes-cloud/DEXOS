using DEXOS.Domain.Common;
using DEXOS.Domain.Events;
using DEXOS.Domain.ValueObjects;

namespace DEXOS.Domain.Entities;

/// <summary>
/// Agregado raíz principal del dominio. Toda la operación del negocio se inicia desde una Orden.
/// </summary>
public class Order
{
    private readonly List<OrderItem> _items = new();
    private readonly List<IDomainEvent> _domainEvents = new();

    private Order()
    {
        Number = string.Empty;
        Total = Money.Zero();
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TenantId { get; private set; }
    public Guid BranchId { get; private set; }
    public Guid? CustomerId { get; private set; }
    public string Number { get; private set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Created;
    public Money Total { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public Order(Guid tenantId, Guid branchId, string number, Guid? customerId = null)
    {
        if (tenantId == Guid.Empty)
        {
            throw new ArgumentException("TenantId es obligatorio.", nameof(tenantId));
        }

        if (branchId == Guid.Empty)
        {
            throw new ArgumentException("BranchId es obligatorio.", nameof(branchId));
        }

        if (string.IsNullOrWhiteSpace(number))
        {
            throw new ArgumentException("El número de orden es obligatorio.", nameof(number));
        }

        TenantId = tenantId;
        BranchId = branchId;
        CustomerId = customerId;
        Number = number.Trim();
        Total = Money.Zero();
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = CreatedAt;
        AddDomainEvent(new OrderCreatedDomainEvent(Id, TenantId, BranchId, Number));
    }

    public void AddItem(OrderItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        _items.Add(item);
        RecalculateTotal();
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Confirm()
    {
        if (!_items.Any())
        {
            throw new InvalidOperationException("Una orden debe tener al menos un ítem antes de confirmarse.");
        }

        if (Status is OrderStatus.Cancelled or OrderStatus.Failed or OrderStatus.Refunded or OrderStatus.Completed)
        {
            throw new InvalidOperationException("No se puede confirmar una orden en estado terminal.");
        }

        if (Status is not OrderStatus.Created and not OrderStatus.PendingPayment)
        {
            throw new InvalidOperationException("La orden ya está siendo procesada.");
        }

        Status = OrderStatus.PendingPayment;
        UpdatedAt = DateTimeOffset.UtcNow;
        AddDomainEvent(new OrderPaymentPendingDomainEvent(Id, TenantId, BranchId, Number));
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Paid || Status == OrderStatus.SentToKitchen || Status == OrderStatus.Preparing || Status == OrderStatus.Ready || Status == OrderStatus.Delivering || Status == OrderStatus.Delivered || Status == OrderStatus.Completed)
        {
            throw new InvalidOperationException("No se puede cancelar una orden ya en proceso o completada.");
        }

        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkPendingPayment()
    {
        if (Status != OrderStatus.Created)
            throw new InvalidOperationException("Solo una orden creada puede pasar a pendiente de pago.");

        Status = OrderStatus.PendingPayment;
        UpdatedAt = DateTimeOffset.UtcNow;
        AddDomainEvent(new OrderPaymentPendingDomainEvent(Id, TenantId, BranchId, Number));
    }

    public void MarkPaid(string paymentReference)
    {
        if (Status != OrderStatus.PendingPayment)
            throw new InvalidOperationException("Solo una orden en estado PendingPayment puede marcarse como pagada.");

        if (string.IsNullOrWhiteSpace(paymentReference))
            throw new ArgumentException("Payment reference es requerido.", nameof(paymentReference));

        Status = OrderStatus.Paid;
        UpdatedAt = DateTimeOffset.UtcNow;
        AddDomainEvent(new OrderPaidDomainEvent(Id, TenantId, BranchId, Number, paymentReference));
    }

    public void SendToKitchen()
    {
        if (Status != OrderStatus.Paid)
            throw new InvalidOperationException("No se puede enviar a cocina si la orden no está pagada.");

        Status = OrderStatus.SentToKitchen;
        UpdatedAt = DateTimeOffset.UtcNow;
        AddDomainEvent(new OrderSentToKitchenDomainEvent(Id, TenantId, BranchId, Number));
    }

    public void StartPreparation()
    {
        if (Status != OrderStatus.SentToKitchen)
            throw new InvalidOperationException("La orden debe estar enviada a cocina para comenzar preparación.");

        Status = OrderStatus.Preparing;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkReady()
    {
        if (Status != OrderStatus.Preparing)
            throw new InvalidOperationException("La orden debe estar en preparación para marcarla como lista.");

        Status = OrderStatus.Ready;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void StartDelivery()
    {
        if (Status != OrderStatus.Ready)
            throw new InvalidOperationException("La orden debe estar lista para iniciar la entrega.");

        Status = OrderStatus.Delivering;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void MarkDelivered()
    {
        if (Status != OrderStatus.Delivering)
            throw new InvalidOperationException("La orden debe estar en entrega para marcarla como entregada.");

        Status = OrderStatus.Delivered;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Complete()
    {
        if (Status != OrderStatus.Delivered)
            throw new InvalidOperationException("La orden debe estar entregada para completarla.");

        Status = OrderStatus.Completed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Fail(string? reason = null)
    {
        if (Status is OrderStatus.Cancelled or OrderStatus.Refunded or OrderStatus.Failed or OrderStatus.Completed)
            throw new InvalidOperationException("No se puede marcar como fallida una orden en estado terminal.");

        Status = OrderStatus.Failed;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void Refund()
    {
        if (Status is not OrderStatus.Paid and not OrderStatus.Delivered and not OrderStatus.Completed)
            throw new InvalidOperationException("Solo las órdenes pagadas o completadas pueden reembolsarse.");

        Status = OrderStatus.Refunded;
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    private void RecalculateTotal()
    {
        Total = _items.Aggregate(Money.Zero(), (current, item) => current + item.Total);
    }

    private void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}
