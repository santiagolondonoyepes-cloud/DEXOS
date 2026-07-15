using DEXOS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEXOS.Infrastructure.Persistence.Mappings;

/// <summary>
/// Configuración EF Core para la entidad Order con soporte multi-tenant.
/// </summary>
public sealed class OrderMapping : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id).ValueGeneratedNever();
        builder.Property(o => o.TenantId).IsRequired();
        builder.Property(o => o.BranchId).IsRequired();
        builder.Property(o => o.Number).HasMaxLength(50).IsRequired();
        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(o => o.CreatedAt).IsRequired();
        builder.Property(o => o.UpdatedAt).IsRequired();

        // Value Object Money mapeado como propiedad compleja
        builder.ComplexProperty(o => o.Total, moneyBuilder =>
        {
            moneyBuilder.Property(m => m.Amount)
                .HasColumnName("TotalAmount")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        // Items como colección de propiedades
        builder.HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey("OrderId")
            .OnDelete(DeleteBehavior.Cascade);

        // Índices para consultas frecuentes
        builder.HasIndex(o => o.TenantId);
        builder.HasIndex(o => o.BranchId);
        builder.HasIndex(o => o.CreatedAt);
        builder.HasIndex(o => new { o.TenantId, o.Status });
    }
}
