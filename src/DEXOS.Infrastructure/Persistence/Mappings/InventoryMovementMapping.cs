using DEXOS.Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEXOS.Infrastructure.Persistence.Mappings;

/// <summary>
/// Configuración EF Core para la entidad InventoryMovement (Kardex).
/// </summary>
public sealed class InventoryMovementMapping : IEntityTypeConfiguration<InventoryMovement>
{
    public void Configure(EntityTypeBuilder<InventoryMovement> builder)
    {
        builder.HasKey(im => im.Id);

        builder.Property(im => im.Id).ValueGeneratedNever();
        builder.Property(im => im.ProductId).IsRequired();
        builder.Property(im => im.TenantId).IsRequired();
        builder.Property(im => im.Type).HasMaxLength(20).IsRequired();
        builder.Property(im => im.Quantity).IsRequired();
        builder.Property(im => im.Reason).HasMaxLength(500);
        builder.Property(im => im.OccurredAt).IsRequired();

        builder.HasIndex(im => new { im.TenantId, im.ProductId, im.OccurredAt });
        builder.HasIndex(im => im.OccurredAt);

        builder.ToTable("InventoryMovements");
    }
}
