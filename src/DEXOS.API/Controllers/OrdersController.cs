using DEXOS.API.Requests;
using DEXOS.Application.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DEXOS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "Operations")]
public sealed class OrdersController : ControllerBase
{
    private readonly CreateOrderUseCase _createOrderUseCase;
    private readonly GetOrdersUseCase _getOrdersUseCase;
    private readonly GetOrderByIdUseCase _getOrderByIdUseCase;
    private readonly AddOrderItemUseCase _addOrderItemUseCase;
    private readonly ConfirmOrderUseCase _confirmOrderUseCase;
    private readonly InitiatePaymentUseCase _initiatePaymentUseCase;
    private readonly ConfirmPaymentUseCase _confirmPaymentUseCase;
    private readonly SendOrderToKitchenUseCase _sendOrderToKitchenUseCase;

    public OrdersController(
        CreateOrderUseCase createOrderUseCase,
        GetOrdersUseCase getOrdersUseCase,
        GetOrderByIdUseCase getOrderByIdUseCase,
        AddOrderItemUseCase addOrderItemUseCase,
        ConfirmOrderUseCase confirmOrderUseCase,
        InitiatePaymentUseCase initiatePaymentUseCase,
        ConfirmPaymentUseCase confirmPaymentUseCase,
        SendOrderToKitchenUseCase sendOrderToKitchenUseCase)
    {
        _createOrderUseCase = createOrderUseCase;
        _getOrdersUseCase = getOrdersUseCase;
        _getOrderByIdUseCase = getOrderByIdUseCase;
        _addOrderItemUseCase = addOrderItemUseCase;
        _confirmOrderUseCase = confirmOrderUseCase;
        _initiatePaymentUseCase = initiatePaymentUseCase;
        _confirmPaymentUseCase = confirmPaymentUseCase;
        _sendOrderToKitchenUseCase = sendOrderToKitchenUseCase;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderRequest request)
    {
        var items = request.Items?.Select(item => new CreateOrderItemCommand(item.ProductId, item.Description, item.Quantity, item.UnitPrice)).ToList()
            ?? new List<CreateOrderItemCommand>();

        var command = new CreateOrderCommand(request.TenantId, request.BranchId, request.Number, request.CustomerId, items);
        var order = await _createOrderUseCase.ExecuteAsync(command);
        return CreatedAtAction(nameof(GetById), new { orderId = order.Id }, order.ToResponse());
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid tenantId)
    {
        var orders = await _getOrdersUseCase.ExecuteAsync(tenantId);
        return Ok(orders.Select(order => order.ToResponse()));
    }

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetById(Guid orderId)
    {
        var order = await _getOrderByIdUseCase.ExecuteAsync(orderId);
        return Ok(order.ToResponse());
    }

    [HttpPost("{orderId:guid}/items")]
    public async Task<IActionResult> AddItem(Guid orderId, AddOrderItemRequest request)
    {
        var command = new AddOrderItemCommand(orderId, request.ProductId, request.Description, request.Quantity, request.UnitPrice);
        var order = await _addOrderItemUseCase.ExecuteAsync(command);
        return Ok(order);
    }

    [HttpPost("{orderId:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid orderId)
    {
        var command = new ConfirmOrderCommand(orderId);
        var order = await _confirmOrderUseCase.ExecuteAsync(command);
        return Ok(order);
    }

    [HttpPost("{orderId:guid}/payments/initiate")]
    public async Task<IActionResult> InitiatePayment(Guid orderId, [FromBody] InitiatePaymentRequest request)
    {
        var command = new InitiatePaymentCommand(orderId, request.TenantId, request.Provider, request.Amount, request.Currency, request.Description, request.ReturnUrl);
        var order = await _initiatePaymentUseCase.ExecuteAsync(command);
        return Ok(order);
    }

    [HttpPost("{orderId:guid}/payments/webhook")]
    public async Task<IActionResult> PaymentWebhook(Guid orderId, [FromBody] PaymentWebhookRequest request)
    {
        var command = new ConfirmPaymentCommand(orderId, request.TenantId, request.Provider, request.PaymentReference, request.EventType);
        var order = await _confirmPaymentUseCase.ExecuteAsync(command);
        return Ok(order);
    }

    [HttpPost("{orderId:guid}/send-to-kitchen")]
    public async Task<IActionResult> SendToKitchen(Guid orderId, [FromQuery] Guid tenantId)
    {
        var command = new SendOrderToKitchenCommand(orderId, tenantId);
        var order = await _sendOrderToKitchenUseCase.ExecuteAsync(command);
        return Ok(order);
    }
}
