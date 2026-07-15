using DEXOS.CRM.Abstractions;
using DEXOS.CRM.Models;

namespace DEXOS.CRM.Services;

/// <summary>
/// Servicio de CRM para gestión de clientes, segmentación y fidelización.
/// </summary>
public class CustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Customer> CreateCustomerAsync(Guid tenantId, string fullName, string email, string phone, CancellationToken cancellationToken = default)
    {
        var customer = new Customer(tenantId, fullName, email, phone);
        return await _repository.AddAsync(customer, cancellationToken);
    }

    public async Task<Customer> AddLoyaltyPointsAsync(Guid customerId, int points, CancellationToken cancellationToken = default)
    {
        var customer = await _repository.GetByIdAsync(customerId, cancellationToken)
            ?? throw new KeyNotFoundException("Cliente no encontrado.");
        customer.AddLoyaltyPoints(points);
        await _repository.UpdateAsync(customer, cancellationToken);
        return customer;
    }

    public async Task<Customer> SetSegmentAsync(Guid customerId, string segment, CancellationToken cancellationToken = default)
    {
        var customer = await _repository.GetByIdAsync(customerId, cancellationToken)
            ?? throw new KeyNotFoundException("Cliente no encontrado.");
        customer.SetSegment(segment);
        await _repository.UpdateAsync(customer, cancellationToken);
        return customer;
    }
}
