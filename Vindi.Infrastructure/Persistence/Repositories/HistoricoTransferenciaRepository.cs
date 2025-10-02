using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Vindi.Core.Entities;
using Vindi.Core.Interfaces;

namespace Vindi.Infrastructure.Persistence.Repositories;

public class HistoricoTransferenciaRepository : IHistoricoTransferenciaRepository
{
    private readonly AppDbContext _context;

    public HistoricoTransferenciaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(HistoricoTransferencia historico)
    {
        await _context.HistoricoTransferencias.AddAsync(historico);
    }

    public async Task<IEnumerable<HistoricoTransferencia>> ObterPorDocumentoAsync(string documento)
    {
        return await _context.HistoricoTransferencias
            .Where(h => h.DocumentoOrigem == documento || h.DocumentoDestino == documento)
            .OrderByDescending(h => h.Timestamp)
            .ToListAsync();
    }
}