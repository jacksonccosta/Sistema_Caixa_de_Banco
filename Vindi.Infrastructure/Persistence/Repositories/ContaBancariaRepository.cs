using Microsoft.EntityFrameworkCore;
using Vindi.Core.Entities;
using Vindi.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vindi.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementação do repositório para a entidade ContaBancaria,
/// responsável por abstrair o acesso a dados.
/// </summary>
public class ContaBancariaRepository : IContaBancariaRepository
{
    private readonly AppDbContext _context;

    public ContaBancariaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(ContaBancaria contaBancaria)
    {
        await _context.ContasBancarias.AddAsync(contaBancaria);
    }

    public async Task<bool> DocumentoJaExisteAsync(string documento)
    {
        return await _context.ContasBancarias.AnyAsync(c => c.DocumentoCliente == documento);
    }

    public async Task<ContaBancaria?> ObterPorDocumentoAsync(string documento)
    {
        return await _context.ContasBancarias.SingleOrDefaultAsync(c => c.DocumentoCliente == documento);
    }

    public async Task<ContaBancaria?> ObterPorIdAsync(Guid id)
    {
        return await _context.ContasBancarias.FindAsync(id);
    }

    public async Task<IEnumerable<ContaBancaria>> ListarContasAsync(string? nome, string? documento)
    {
        var query = _context.ContasBancarias.AsQueryable();

        if (!string.IsNullOrWhiteSpace(nome))
        {
            query = query.Where(c => c.NomeCliente.Contains(nome));
        }

        if (!string.IsNullOrWhiteSpace(documento))
        {
            query = query.Where(c => c.DocumentoCliente == documento);
        }

        return await query.ToListAsync();
    }
}