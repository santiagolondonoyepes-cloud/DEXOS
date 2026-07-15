using DEXOS.Application.Orders;
using DEXOS.Application.Abstractions;
using DEXOS.Payments;
using Microsoft.Extensions.DependencyInjection;

namespace DEXOS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateOrderUseCase>();
        services.AddScoped<GetOrdersUseCase>();
        services.AddScoped<GetOrderByIdUseCase>();
        services.AddScoped<AddOrderItemUseCase>();
        services.AddScoped<ConfirmOrderUseCase>();
        services.AddScoped<InitiatePaymentUseCase>();
        services.AddScoped<ConfirmPaymentUseCase>();
        services.AddScoped<SendOrderToKitchenUseCase>();
        services.AddScoped<OrderService>();
        services.AddScoped<IKitchenStationRouter, DefaultKitchenStationRouter>();
        services.AddScoped<IRealtimeNotifier, NullRealtimeNotifier>();
        services.AddScoped<IPaymentService, SimulatedPaymentService>();

        return services;
    }
}
