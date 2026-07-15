namespace DEXOS.Domain.Common;

/// <summary>
/// Marca un evento de dominio que puede ser procesado por handlers o middleware de infraestructura.
/// </summary>
public interface IDomainEvent
{
    Guid AggregateId { get; }
    DateTimeOffset OccurredAt { get; }
}
