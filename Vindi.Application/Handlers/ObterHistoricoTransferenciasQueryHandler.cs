using MediatR;
using Vindi.Application.DTOs;
using Vindi.Application.Queries;
using Vindi.Core.Interfaces;

namespace Vindi.Application.Handlers;

public class ObterHistoricoTransferenciasQueryHandler(IHistoricoTransferenciaRepository repository) : IRequestHandler<ObterHistoricoTransferenciasQuery, IEnumerable<TransferenciaDto>>
{
    private readonly IHistoricoTransferenciaRepository _repository = repository;

    public async Task<IEnumerable<TransferenciaDto>> Handle(ObterHistoricoTransferenciasQuery request, CancellationToken cancellationToken)
    {
        var historico = await _repository.ObterPorDocumentoAsync(request.Documento);

        return historico.Select(h => new TransferenciaDto
        {
            Id = h.Id,
            Timestamp = h.Timestamp.ToString("o"),
            Valor = h.Valor,
            DocumentoOrigem = h.DocumentoOrigem,
            DocumentoDestino = h.DocumentoDestino,
            Tipo = h.DocumentoOrigem == request.Documento ? "Enviada" : "Recebida"
        });
    }
}