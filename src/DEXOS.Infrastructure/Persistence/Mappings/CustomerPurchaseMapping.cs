using DEXOS.CRM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEXOS.Infrastructure.Persistence.Mappings;

public sealed class CustomerPurchaseMapping : IEntityTypeConfiguration<CustomerPurchase>
{
    public void Configure(EntityTypeBuilder<CustomerPurchase> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.OrderId).IsRequired();
        builder.Property(x => x.Amount).HasPrecision(18, 2).IsRequired();
        builder.Property(x => x.PurchasedAt).IsRequired();

        builder.HasIndex(x => new { x.TenantId, x.CustomerId, x.PurchasedAt });
        builder.ToTable("CustomerPurchases");
    }
}
