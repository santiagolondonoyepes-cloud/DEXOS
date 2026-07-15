using DEXOS.CRM.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEXOS.Infrastructure.Persistence.Mappings;

public sealed class QrMenuConfigurationMapping : IEntityTypeConfiguration<QrMenuConfiguration>
{
    public void Configure(EntityTypeBuilder<QrMenuConfiguration> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.TenantId).IsRequired();
        builder.Property(x => x.BranchId);
        builder.Property(x => x.Mode).HasMaxLength(20).IsRequired();
        builder.Property(x => x.QrToken).HasMaxLength(80).IsRequired();
        builder.Property(x => x.InternalPath).HasMaxLength(500);
        builder.Property(x => x.ExternalUrl).HasMaxLength(1000);
        builder.Property(x => x.WebhookSecret).HasMaxLength(200);
        builder.Property(x => x.UpdatedAt).IsRequired();

        builder.HasIndex(x => x.QrToken).IsUnique();
        builder.HasIndex(x => new { x.TenantId, x.BranchId }).IsUnique();

        builder.ToTable("QrMenuConfigurations");
    }
}
