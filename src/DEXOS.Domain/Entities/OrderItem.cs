using DEXOS.Domain.ValueObjects;

namespace DEXOS.Domain.Entities;

public class OrderItem
{
    private OrderItem()
    {
        Description = string.Empty;
        UnitPrice = Money.Zero();
    }

    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid ProductId { get; private set; }
    public string Description { get; private set; }
    public int Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money Total => UnitPrice * Quantity;

    public OrderItem(Guid productId, string description, int quantity, Money unitPrice)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("ProductId es obligatorio.", nameof(productId));
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("La descripción del ítem es obligatoria.", nameof(description));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "La cantidad debe ser mayor a cero.");
        }

        if (unitPrice.Amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(unitPrice), "El precio unitario debe ser mayor que cero.");
        }

        ProductId = productId;
        Description = description.Trim();
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}
