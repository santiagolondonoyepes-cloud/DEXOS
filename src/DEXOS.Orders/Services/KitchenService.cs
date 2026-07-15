using DEXOS.Orders.Abstractions;
using DEXOS.Orders.Models;

namespace DEXOS.Orders.Services;

/// <summary>
/// Servicio encargado de generar, actualizar y distribuir comandas digitales.
/// </summary>
public class KitchenService
{
    private readonly IKitchenOrderRepository _repository;

    public KitchenService(IKitchenOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task<KitchenOrder> CreateKitchenOrderAsync(Guid orderId, Guid tenantId, string station, string? notes = null, CancellationToken cancellationToken = default)
    {
        if (orderId == Guid.Empty) throw new ArgumentException("OrderId es obligatorio.", nameof(orderId));
        var entity = new KitchenOrder(orderId, tenantId, station, notes);
        return await _repository.AddAsync(entity, cancellationToken);
    }

    public async Task<KitchenOrder> MarkReadyAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException("Comanda no encontrada.");
        entity.MarkReady();
        await _repository.UpdateAsync(entity, cancellationToken);
        return entity;
    }

    public async Task<KitchenOrder> MarkDeliveredAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new KeyNotFoundException("Comanda no encontrada.");
        entity.MarkDelivered();
        await _repository.UpdateAsync(entity, cancellationToken);
        return entity;
    }
}
