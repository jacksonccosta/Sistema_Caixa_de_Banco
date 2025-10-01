using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vindi.Core.Entities;

namespace Vindi.Infrastructure.Persistence.Configurations;

public class ContaBancariaConfiguration : IEntityTypeConfiguration<ContaBancaria>
{
    public void Configure(EntityTypeBuilder<ContaBancaria> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.NomeCliente)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.DocumentoCliente)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(c => c.DocumentoCliente)
            .IsUnique();

        builder.Property(c => c.Saldo)
            .HasColumnType("decimal(18,2)");

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(20);
    }
}