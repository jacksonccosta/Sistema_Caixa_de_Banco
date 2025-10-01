using MediatR;
using Vindi.Application.Commands;
using Vindi.Core.Interfaces;

namespace Vindi.Application.Handlers;

public class RealizarTransferenciaCommandHandler : IRequestHandler<RealizarTransferenciaCommand>
{
    private readonly IContaBancariaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public RealizarTransferenciaCommandHandler(IContaBancariaRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RealizarTransferenciaCommand request, CancellationToken cancellationToken)
    {
        var contaOrigem = await _repository.ObterPorDocumentoAsync(request.DocumentoOrigem)
            ?? throw new KeyNotFoundException("Conta de origem não encontrada.");

        var contaDestino = await _repository.ObterPorDocumentoAsync(request.DocumentoDestino)
            ?? throw new KeyNotFoundException("Conta de destino não encontrada.");

        if (contaOrigem.Id == contaDestino.Id)
            throw new InvalidOperationException("A conta de origem e destino não podem ser a mesma.");

        contaOrigem.Debitar(request.Valor);
        contaDestino.Creditar(request.Valor);

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}