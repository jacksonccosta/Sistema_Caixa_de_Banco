using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Vindi.Core.Entities;

namespace Vindi.Infrastructure.Persistence.Configurations;

public class LogAuditoriaConfiguration : IEntityTypeConfiguration<LogAuditoria>
{
    public void Configure(EntityTypeBuilder<LogAuditoria> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Acao).IsRequired().HasMaxLength(100);

        builder.Property(l => l.UsuarioResponsavel).IsRequired().HasMaxLength(100);

        builder.Property(l => l.Detalhes).IsRequired().HasMaxLength(1000);

        builder.Property(l => l.Timestamp).IsRequired();
    }
}