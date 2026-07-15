using DEXOS.Application.Abstractions;
using DEXOS.CRM.Abstractions;
using DEXOS.CRM.Models;
using DEXOS.Inventory.Abstractions;
using DEXOS.Inventory.Models;
using DEXOS.Orders.Abstractions;
using DEXOS.Orders.Models;
using DEXOS.Payments;

namespace DEXOS.Application.Orders;

/// <summary>
/// Caso de uso para confirmar un pago desde la pasarela o un webhook simulado.
/// </summary>
public sealed class ConfirmPaymentUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;
    private readonly IInventoryRepository _inventoryRepository;
    private readonly IKitchenOrderRepository _kitchenOrderRepository;
    private readonly IKitchenStationRouter _kitchenStationRouter;
    private readonly IRealtimeNotifier _realtimeNotifier;
    private readonly ICustomerRepository _customerRepository;

    public ConfirmPaymentUseCase(
        IOrderRepository orderRepository,
        IPaymentService paymentService,
        IInventoryRepository inventoryRepository,
        IKitchenOrderRepository kitchenOrderRepository,
        IKitchenStationRouter kitchenStationRouter,
        IRealtimeNotifier realtimeNotifier,
        ICustomerRepository customerRepository)
    {
        _orderRepository = orderRepository;
        _paymentService = paymentService;
        _inventoryRepository = inventoryRepository;
        _kitchenOrderRepository = kitchenOrderRepository;
        _kitchenStationRouter = kitchenStationRouter;
        _realtimeNotifier = realtimeNotifier;
        _customerRepository = customerRepository;
    }

    public async Task<OrderResponse> ExecuteAsync(ConfirmPaymentCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Orden no encontrada.");

        if (order.TenantId != command.TenantId)
        {
            throw new InvalidOperationException("La orden no pertenece al tenant indicado.");
        }

        var confirmationRequest = new PaymentConfirmationRequest(order.Id, order.TenantId, command.PaymentReference, command.Provider);
        var paymentResult = await _paymentService.ConfirmPaymentAsync(confirmationRequest, cancellationToken);

        if (!paymentResult.IsSuccess)
        {
            throw new InvalidOperationException(paymentResult.Message ?? "No se pudo confirmar el pago.");
        }

        order.MarkPaid(paymentResult.PaymentReference);

        // Inventory decrement as soon as sale is confirmed paid.
        foreach (var item in order.Items)
        {
            var product = await _inventoryRepository.GetProductByIdAsync(item.ProductId, cancellationToken)
                ?? throw new InvalidOperationException($"Producto {item.ProductId} no encontrado para decrementar inventario.");

            product.AdjustStock(-item.Quantity);
            await _inventoryRepository.UpdateProductAsync(product, cancellationToken);

            var movement = new InventoryMovement(
                productId: product.Id,
                tenantId: order.TenantId,
                type: "SALE",
                quantity: item.Quantity,
                reason: $"Venta confirmada para orden {order.Number}");

            await _inventoryRepository.AddMovementAsync(movement, cancellationToken);
        }

        // Create kitchen tickets grouped by station once payment is confirmed.
        var groupedItems = order.Items.GroupBy(_kitchenStationRouter.ResolveStation);
        foreach (var group in groupedItems)
        {
            var notes = string.Join(", ", group.Select(i => $"{i.Quantity}x {i.Description}"));
            var kitchenOrder = new KitchenOrder(order.Id, order.TenantId, group.Key, notes);
            await _kitchenOrderRepository.AddAsync(kitchenOrder, cancellationToken);

            await _realtimeNotifier.NotifyKitchenOrderCreatedAsync(order.TenantId, order.BranchId, order.Id, group.Key, cancellationToken);
        }

        await _orderRepository.UpdateAsync(order, cancellationToken);

        // CRM purchase history and loyalty automation.
        if (order.CustomerId.HasValue)
        {
            var customer = await _customerRepository.GetByIdAsync(order.CustomerId.Value, cancellationToken);
            if (customer is not null)
            {
                var purchase = new CustomerPurchase(order.TenantId, customer.Id, order.Id, order.Total.Amount);
                await _customerRepository.AddPurchaseAsync(purchase, cancellationToken);

                customer.RegisterPurchase(order.Total.Amount);
                await _customerRepository.UpdateAsync(customer, cancellationToken);

                // Loyalty coupon issuance rule: each 1000 points unlocks a 10% coupon.
                if (customer.LoyaltyPoints >= 1000 && customer.LoyaltyPoints % 1000 < (int)Math.Floor(order.Total.Amount))
                {
                    var coupon = new LoyaltyCoupon(
                        tenantId: order.TenantId,
                        customerId: customer.Id,
                        code: $"DX-{order.Number}-{DateTime.UtcNow:yyyyMMddHHmmss}",
                        discountPercentage: 10m,
                        expiresAt: DateTimeOffset.UtcNow.AddDays(30));

                    await _customerRepository.AddCouponAsync(coupon, cancellationToken);
                }
            }
        }

        await _realtimeNotifier.NotifyOrderPaidAsync(order.TenantId, order.BranchId, order.Id, order.Number, cancellationToken);

        return order.ToResponse();
    }
}
