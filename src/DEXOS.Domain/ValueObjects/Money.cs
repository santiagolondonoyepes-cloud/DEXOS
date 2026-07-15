namespace DEXOS.Domain.ValueObjects;

/// <summary>
/// Value object para representar montos con precisión decimal y semántica de dominio.
/// </summary>
public readonly record struct Money(decimal Amount)
{
    public static Money Zero() => new(0m);

    public static Money FromDecimal(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "El monto no puede ser negativo.");
        }

        return new Money(amount);
    }

    public static Money operator +(Money left, Money right) => new(left.Amount + right.Amount);
    public static Money operator *(Money left, int multiplier)
    {
        if (multiplier < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(multiplier), "El multiplicador no puede ser negativo.");
        }

        return new Money(left.Amount * multiplier);
    }

    public static Money operator *(int multiplier, Money right) => right * multiplier;

    public override string ToString() => Amount.ToString("0.00");
}
