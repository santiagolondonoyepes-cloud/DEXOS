using DEXOS.Orders.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEXOS.Infrastructure.Persistence.Mappings;

/// <summary>
/// Configuración EF Core para la entidad KitchenOrder.
/// </summary>
public sealed class KitchenOrderMapping : IEntityTypeConfiguration<KitchenOrder>
{
    public void Configure(EntityTypeBuilder<KitchenOrder> builder)
    {
        builder.HasKey(ko => ko.Id);

        builder.Property(ko => ko.Id).ValueGeneratedNever();
        builder.Property(ko => ko.OrderId).IsRequired();
        builder.Property(ko => ko.TenantId).IsRequired();
        builder.Property(ko => ko.Station).HasMaxLength(50).HasDefaultValue("kitchen");
        builder.Property(ko => ko.Status).HasMaxLength(20).HasDefaultValue("PREPARING");
        builder.Property(ko => ko.Notes).HasMaxLength(1000);

        builder.HasIndex(ko => new { ko.TenantId, ko.Status });
        builder.HasIndex(ko => ko.OrderId);

        builder.ToTable("KitchenOrders");
    }
}
