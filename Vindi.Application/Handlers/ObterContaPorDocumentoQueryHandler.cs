using MediatR;
using Vindi.Application.DTOs;
using Vindi.Application.Queries;
using Vindi.Core.Interfaces;

namespace Vindi.Application.Handlers;

public class ObterContaPorDocumentoQueryHandler(IContaBancariaRepository repository) : IRequestHandler<ObterContaPorDocumentoQuery, ContaDto?>
{
    private readonly IContaBancariaRepository _repository = repository;

    public async Task<ContaDto?> Handle(ObterContaPorDocumentoQuery request, CancellationToken cancellationToken)
    {
        var conta = await _repository.ObterPorDocumentoAsync(request.Documento);

        if (conta is null)
        {
            return null;
        }

        return new ContaDto
        {
            Id = conta.Id,
            NomeCliente = conta.NomeCliente,
            DocumentoCliente = conta.DocumentoCliente,
            SaldoAtual = conta.Saldo,
            DataAbertura = conta.DataAbertura.ToString("o"),
            Status = conta.Status.ToString()
        };
    }
}