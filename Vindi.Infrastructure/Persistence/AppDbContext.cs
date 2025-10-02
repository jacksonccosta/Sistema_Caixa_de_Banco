using Microsoft.EntityFrameworkCore;
using Vindi.Core.Entities;
using System.Reflection;

namespace Vindi.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<ContaBancaria> ContasBancarias { get; set; }
    public DbSet<LogAuditoria> LogsAuditoria { get; set; }
    public DbSet<HistoricoTransferencia> HistoricoTransferencias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}