using DEXOS.Domain.Entities;

namespace DEXOS.Application.Orders;

public static class OrderMapper
{
    public static OrderResponse ToResponse(this Order order)
    {
        return new OrderResponse(
            order.Id,
            order.TenantId,
            order.BranchId,
            order.Number,
            order.Status.ToString(),
            order.Total.Amount,
            order.CreatedAt,
            order.UpdatedAt,
            order.Items.Select(item => new OrderItemResponse(
                item.Id,
                item.ProductId,
                item.Description,
                item.Quantity,
                item.UnitPrice.Amount,
                item.Total.Amount)).ToList());
    }
}
