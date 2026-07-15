using DEXOS.CRM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEXOS.Infrastructure.Persistence.Mappings;

/// <summary>
/// Configuración EF Core para la entidad Customer.
/// </summary>
public sealed class CustomerMapping : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();
        builder.Property(c => c.TenantId).IsRequired();
        builder.Property(c => c.FullName).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Phone).HasMaxLength(20);
        builder.Property(c => c.Segment).HasMaxLength(50).HasDefaultValue("new");
        builder.Property(c => c.LoyaltyPoints).HasDefaultValue(0);
        builder.Property(c => c.LifetimeValue).HasPrecision(18, 2).HasDefaultValue(0m);
        builder.Property(c => c.PurchaseCount).HasDefaultValue(0);
        builder.Property(c => c.LastPurchaseAt);

        builder.HasIndex(c => new { c.TenantId, c.Email }).IsUnique();
        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => new { c.TenantId, c.Segment });

        builder.ToTable("Customers");
    }
}
