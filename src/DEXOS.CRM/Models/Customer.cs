namespace DEXOS.CRM.Models;

/// <summary>
/// Cliente del sistema CRM con historial y preferencias.
/// </summary>
public class Customer
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid TenantId { get; private set; }
    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public string Segment { get; private set; } = "new";
    public int LoyaltyPoints { get; private set; }
    public decimal LifetimeValue { get; private set; }
    public int PurchaseCount { get; private set; }
    public DateTimeOffset? LastPurchaseAt { get; private set; }
    public List<string> Preferences { get; private set; } = new();

    public Customer(Guid tenantId, string fullName, string email, string phone)
    {
        if (tenantId == Guid.Empty) throw new ArgumentException("TenantId es obligatorio.", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(fullName)) throw new ArgumentException("Nombre es obligatorio.", nameof(fullName));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email es obligatorio.", nameof(email));

        TenantId = tenantId;
        FullName = fullName.Trim();
        Email = email.Trim();
        Phone = phone?.Trim() ?? string.Empty;
    }

    public void AddLoyaltyPoints(int points) => LoyaltyPoints += points;
    public void SetSegment(string segment) => Segment = segment.Trim();

    public void RegisterPurchase(decimal amount)
    {
        if (amount <= 0m)
        {
            throw new ArgumentOutOfRangeException(nameof(amount));
        }

        LifetimeValue += amount;
        PurchaseCount += 1;
        LastPurchaseAt = DateTimeOffset.UtcNow;

        var loyaltyEarned = (int)Math.Floor(amount);
        AddLoyaltyPoints(loyaltyEarned);

        Segment = ResolveSegment(LifetimeValue, PurchaseCount);
    }

    public static string ResolveSegment(decimal lifetimeValue, int purchaseCount)
    {
        if (lifetimeValue >= 5000m || purchaseCount >= 50) return "champion";
        if (lifetimeValue >= 2000m || purchaseCount >= 20) return "gold";
        if (lifetimeValue >= 500m || purchaseCount >= 8) return "silver";
        return "new";
    }
}
