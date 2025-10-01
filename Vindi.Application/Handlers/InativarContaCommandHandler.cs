using MediatR;
using Vindi.Application.Commands;
using Vindi.Core.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic; // Para KeyNotFoundException

namespace Vindi.Application.Handlers;

public class InativarContaCommandHandler : IRequestHandler<InativarContaCommand>
{
    private readonly IContaBancariaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public InativarContaCommandHandler(IContaBancariaRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(InativarContaCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repository.ObterPorDocumentoAsync(request.DocumentoCliente)
            ?? throw new KeyNotFoundException("Conta não encontrada para o documento informado.");

        conta.Inativar();

        await _unitOfWork.CompleteAsync(cancellationToken);
    }
}