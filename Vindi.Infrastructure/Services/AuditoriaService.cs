using Vindi.Application.Interfaces;
using Vindi.Core.Entities;
using Vindi.Infrastructure.Persistence;

namespace Vindi.Infrastructure.Services;

public class AuditoriaService(AppDbContext context) : IAuditoriaService
{
    private readonly AppDbContext _context = context;

    public void RegistrarLog(LogAuditoria log)
    {
        _context.LogsAuditoria.Add(log);
    }
}