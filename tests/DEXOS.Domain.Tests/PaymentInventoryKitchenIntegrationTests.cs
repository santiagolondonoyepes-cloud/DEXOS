using DEXOS.Application.Abstractions;
using DEXOS.Application.Orders;
using DEXOS.CRM.Abstractions;
using DEXOS.CRM.Models;
using DEXOS.Domain.Entities;
using DEXOS.Domain.ValueObjects;
using DEXOS.Infrastructure.Repositories;
using DEXOS.Inventory.Abstractions;
using DEXOS.Inventory.Models;
using DEXOS.Orders.Abstractions;
using DEXOS.Orders.Models;
using DEXOS.Payments;
using Xunit;

namespace DEXOS.Domain.Tests;

public class PaymentInventoryKitchenIntegrationTests
{
    [Fact]
    public async Task ConfirmPayment_DecrementsInventory_AndCreatesKitchenTickets()
    {
        var tenantId = Guid.NewGuid();
        var branchId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var orderRepository = new InMemoryOrderRepository();
        var inventoryRepository = new FakeInventoryRepository();
        var kitchenRepository = new FakeKitchenOrderRepository();
        var paymentService = new SimulatedPaymentService();
        var router = new DefaultKitchenStationRouter();
        var notifier = new FakeRealtimeNotifier();
        var customerRepository = new FakeCustomerRepository();

        inventoryRepository.Products[productId] = new Product(tenantId, "SKU-1", "Coffee", "Hot coffee", 3.5m, "unit", 10, 1, 50);

        var order = new Order(tenantId, branchId, "ORD-INT-1");
        order.AddItem(new OrderItem(productId, "Coffee cup", 2, Money.FromDecimal(3.5m)));
        order.Confirm();
        await orderRepository.AddAsync(order);

        var useCase = new ConfirmPaymentUseCase(orderRepository, paymentService, inventoryRepository, kitchenRepository, router, notifier, customerRepository);

        var result = await useCase.ExecuteAsync(new ConfirmPaymentCommand(order.Id, tenantId, "simulated", "pay_001"));

        Assert.Equal("Paid", result.Status);
        Assert.Single(kitchenRepository.Items);
        Assert.Equal("barra", kitchenRepository.Items[0].Station);
        Assert.Equal(8, inventoryRepository.Products[productId].Stock);
        Assert.Equal(1, notifier.OrderPaidNotifications);
        Assert.Equal(1, notifier.KitchenNotifications);
    }

    private sealed class FakeInventoryRepository : IInventoryRepository
    {
        public Dictionary<Guid, Product> Products { get; } = new();
        public List<InventoryMovement> Movements { get; } = new();

        public Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            Products[product.Id] = product;
            return Task.FromResult(product);
        }

        public Task<Product?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            Products.TryGetValue(productId, out var product);
            return Task.FromResult(product);
        }

        public Task<Product?> GetProductBySkuAsync(Guid tenantId, string sku, CancellationToken cancellationToken = default)
        {
            var product = Products.Values.FirstOrDefault(x => x.TenantId == tenantId && x.Sku == sku.ToUpperInvariant());
            return Task.FromResult(product);
        }

        public Task<IReadOnlyCollection<Product>> GetProductsAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<Product>>(Products.Values.Where(x => x.TenantId == tenantId).ToList());
        }

        public Task UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task AddMovementAsync(InventoryMovement movement, CancellationToken cancellationToken = default)
        {
            Movements.Add(movement);
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<InventoryMovement>> GetMovementsAsync(Guid productId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<InventoryMovement>>(Movements.Where(x => x.ProductId == productId).ToList());
        }
    }

    private sealed class FakeKitchenOrderRepository : IKitchenOrderRepository
    {
        public List<KitchenOrder> Items { get; } = new();

        public Task<KitchenOrder> AddAsync(KitchenOrder order, CancellationToken cancellationToken = default)
        {
            Items.Add(order);
            return Task.FromResult(order);
        }

        public Task<KitchenOrder?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Items.FirstOrDefault(x => x.Id == id));
        }

        public Task<IReadOnlyCollection<KitchenOrder>> GetByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<KitchenOrder>>(Items.Where(x => x.TenantId == tenantId).ToList());
        }

        public Task UpdateAsync(KitchenOrder order, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }

    private sealed class FakeRealtimeNotifier : IRealtimeNotifier
    {
        public int OrderPaidNotifications { get; private set; }
        public int KitchenNotifications { get; private set; }

        public Task NotifyOrderPaidAsync(Guid tenantId, Guid branchId, Guid orderId, string orderNumber, CancellationToken cancellationToken = default)
        {
            OrderPaidNotifications++;
            return Task.CompletedTask;
        }

        public Task NotifyKitchenOrderCreatedAsync(Guid tenantId, Guid branchId, Guid orderId, string station, CancellationToken cancellationToken = default)
        {
            KitchenNotifications++;
            return Task.CompletedTask;
        }
    }

    private sealed class FakeCustomerRepository : ICustomerRepository
    {
        public Task<Customer> AddAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(customer);
        }

        public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Customer?>(null);
        }

        public Task<Customer?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Customer?>(null);
        }

        public Task<IReadOnlyCollection<Customer>> GetByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<Customer>>(Array.Empty<Customer>());
        }

        public Task UpdateAsync(Customer customer, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task AddPurchaseAsync(CustomerPurchase purchase, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<CustomerPurchase>> GetPurchasesByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<CustomerPurchase>>(Array.Empty<CustomerPurchase>());
        }

        public Task AddCouponAsync(LoyaltyCoupon coupon, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<LoyaltyCoupon>> GetCouponsByCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<LoyaltyCoupon>>(Array.Empty<LoyaltyCoupon>());
        }
    }
}
