using DEXOS.Domain.Entities;

namespace DEXOS.Application.Abstractions;

/// <summary>
/// Strategy para resolver la estación de cocina destino por item de orden.
/// </summary>
public interface IKitchenStationRouter
{
    string ResolveStation(OrderItem item);
}
