using DEXOS.Application.Abstractions;
using DEXOS.Domain.Entities;

namespace DEXOS.Application.Orders;

/// <summary>
/// Routing heurístico inicial para estaciones de preparación.
/// </summary>
public sealed class DefaultKitchenStationRouter : IKitchenStationRouter
{
    public string ResolveStation(OrderItem item)
    {
        var text = $"{item.Description}".ToLowerInvariant();

        if (text.Contains("cafe") || text.Contains("coffee") || text.Contains("jugo") || text.Contains("cocktail") || text.Contains("bar"))
        {
            return "barra";
        }

        if (text.Contains("cake") || text.Contains("pastel") || text.Contains("dessert") || text.Contains("postre") || text.Contains("bakery"))
        {
            return "pasteleria";
        }

        return "cocina";
    }
}
