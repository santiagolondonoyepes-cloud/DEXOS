using DEXOS.Application.Abstractions;
using DEXOS.Domain.Entities;
using DEXOS.Domain.ValueObjects;
using DEXOS.Payments;

namespace DEXOS.Application.Orders;

/// <summary>
/// Caso de uso para iniciar un flujo de pago simulado para una orden.
/// </summary>
public sealed class InitiatePaymentUseCase
{
    private readonly IOrderRepository _orderRepository;
    private readonly IPaymentService _paymentService;

    public InitiatePaymentUseCase(IOrderRepository orderRepository, IPaymentService paymentService)
    {
        _orderRepository = orderRepository;
        _paymentService = paymentService;
    }

    public async Task<OrderResponse> ExecuteAsync(InitiatePaymentCommand command, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);

        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken)
            ?? throw new KeyNotFoundException("Orden no encontrada.");

        if (order.TenantId != command.TenantId)
        {
            throw new InvalidOperationException("La orden no pertenece al tenant indicado.");
        }

        if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Failed || order.Status == OrderStatus.Refunded)
        {
            throw new InvalidOperationException("No se puede iniciar el pago de una orden en un estado terminal.");
        }

        if (order.Status == OrderStatus.Created)
        {
            order.Confirm();
        }
        else if (order.Status != OrderStatus.PendingPayment)
        {
            throw new InvalidOperationException("El pago solo puede iniciarse para una orden creada o pendiente de pago.");
        }

        var paymentAmount = command.Amount > 0m ? command.Amount : order.Total.Amount;
        var paymentRequest = new PaymentRequest(
            order.Id,
            order.TenantId,
            paymentAmount,
            command.Currency,
            command.Provider,
            command.Description ?? $"Pago de la orden {order.Number}",
            command.ReturnUrl);

        var paymentResult = await _paymentService.CreatePaymentAsync(paymentRequest, cancellationToken);

        if (!paymentResult.IsSuccess)
        {
            throw new InvalidOperationException(paymentResult.Message ?? "No se pudo iniciar el pago.");
        }

        order.MarkPaid(paymentResult.PaymentReference);
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return order.ToResponse();
    }
}
