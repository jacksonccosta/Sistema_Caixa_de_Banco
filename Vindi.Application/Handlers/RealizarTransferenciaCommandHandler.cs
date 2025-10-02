using MediatR;
using Vindi.Application.Commands;
using Vindi.Core.Entities;
using Vindi.Core.Interfaces;

namespace Vindi.Application.Handlers;

public class RealizarTransferenciaCommandHandler : IRequestHandler<RealizarTransferenciaCommand>
{
    private readonly IContaBancariaRepository _contaRepository;
    private readonly IHistoricoTransferenciaRepository _historicoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RealizarTransferenciaCommandHandler(
        IContaBancariaRepository contaRepository,
        IHistoricoTransferenciaRepository historicoRepository,
        IUnitOfWork unitOfWork)
    {
        _contaRepository = contaRepository;
        _historicoRepository = historicoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RealizarTransferenciaCommand request, CancellationToken cancellationToken)
    {
        var contaOrigem = await _contaRepository.ObterPorDocumentoAsync(request.DocumentoOrigem)
            ?? throw new KeyNotFoundException("Conta de origem não encontrada.");

        var contaDestino = await _contaRepository.ObterPorDocumentoAsync(request.DocumentoDestino)
            ?? throw new KeyNotFoundException("Conta de destino não encontrada.");

        if (contaOrigem.Id == contaDestino.Id)
            throw new InvalidOperationException("A conta de origem e destino não podem ser a mesma.");

        contaOrigem.Debitar(request.Valor);
        contaDestino.Creditar(request.Valor);

        var historico = new HistoricoTransferencia(contaOrigem, contaDestino, request.Valor);
        await _historicoRepository.AdicionarAsync(historico);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}