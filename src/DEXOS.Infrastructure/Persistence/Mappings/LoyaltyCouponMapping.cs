using DEXOS.CRM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEXOS.Infrastructure.Persistence.Mappings;

public sealed class LoyaltyCouponMapping : IEntityTypeConfiguration<LoyaltyCoupon>
{
    public void Configure(EntityTypeBuilder<LoyaltyCoupon> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.CustomerId).IsRequired();
        builder.Property(x => x.Code).HasMaxLength(50).IsRequired();
        builder.Property(x => x.DiscountPercentage).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.ExpiresAt).IsRequired();
        builder.Property(x => x.IsRedeemed).IsRequired();

        builder.HasIndex(x => new { x.TenantId, x.Code }).IsUnique();
        builder.ToTable("LoyaltyCoupons");
    }
}
