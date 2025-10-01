using MediatR;
using Vindi.Application.DTOs;
using Vindi.Application.Queries;
using Vindi.Core.Interfaces;

namespace Vindi.Application.Handlers;

public class ListarContasQueryHandler(IContaBancariaRepository repository) : IRequestHandler<ListarContasQuery, IEnumerable<ContaDto>>
{
    private readonly IContaBancariaRepository _repository = repository;

    public async Task<IEnumerable<ContaDto>> Handle(ListarContasQuery request, CancellationToken cancellationToken)
    {
        var contas = await _repository.ListarContasAsync(request.Nome, request.Documento);

        return contas.Select(c => new ContaDto
        {
            Id = c.Id,
            NomeCliente = c.NomeCliente,
            DocumentoCliente = c.DocumentoCliente,
            SaldoAtual = c.Saldo,
            DataAbertura = c.DataAbertura.ToString("o"),
            Status = c.Status.ToString()
        });
    }
}