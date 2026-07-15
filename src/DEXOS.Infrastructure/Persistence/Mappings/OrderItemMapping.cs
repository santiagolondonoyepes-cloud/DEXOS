using DEXOS.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DEXOS.Infrastructure.Persistence.Mappings;

/// <summary>
/// Configuración EF Core para la entidad OrderItem.
/// </summary>
public sealed class OrderItemMapping : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id).ValueGeneratedNever();
        builder.Property(oi => oi.ProductId).IsRequired();
        builder.Property(oi => oi.Description).HasMaxLength(500).IsRequired();
        builder.Property(oi => oi.Quantity).IsRequired();

        // Value Object Money para UnitPrice
        builder.ComplexProperty(oi => oi.UnitPrice, priceBuilder =>
        {
            priceBuilder.Property(m => m.Amount)
                .HasColumnName("UnitPriceAmount")
                .HasPrecision(18, 2)
                .IsRequired();
        });

        builder.ToTable("OrderItems");
    }
}
