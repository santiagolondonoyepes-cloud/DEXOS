using DEXOS.Application.Abstractions;
using DEXOS.CRM.Abstractions;
using DEXOS.Infrastructure.Persistence;
using DEXOS.Infrastructure.Repositories;
using DEXOS.Infrastructure.Repositories.CRM;
using DEXOS.Infrastructure.Repositories.Inventory;
using DEXOS.Infrastructure.Repositories.Orders;
using DEXOS.Inventory.Abstractions;
using DEXOS.Orders.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DEXOS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");

        services.AddDbContext<DexosDbContext>(options =>
            options.UseSqlServer(connectionString, sqlOptions =>
                sqlOptions.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));

        // Repositorios
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IInventoryRepository, InventoryRepository>();
        services.AddScoped<IKitchenOrderRepository, KitchenOrderRepository>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
