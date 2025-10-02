using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vindi.Core.Entities;

namespace Vindi.Infrastructure.Persistence.Configurations;

public class HistoricoTransferenciaConfiguration : IEntityTypeConfiguration<HistoricoTransferencia>
{
    public void Configure(EntityTypeBuilder<HistoricoTransferencia> builder)
    {
        builder.HasKey(h => h.Id);

        builder.Property(h => h.Valor).HasColumnType("decimal(18,2)");

        builder.HasOne(h => h.ContaOrigem)
            .WithMany(c => c.TransferenciasEnviadas)
            .HasForeignKey(h => h.ContaOrigemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(h => h.ContaDestino)
            .WithMany(c => c.TransferenciasRecebidas)
            .HasForeignKey(h => h.ContaDestinoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}