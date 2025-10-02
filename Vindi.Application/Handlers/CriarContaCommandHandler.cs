using MediatR;
using Vindi.Application.Commands;
using Vindi.Application.DTOs;
using Vindi.Core.Entities;
using Vindi.Core.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Vindi.Application.Handlers;

public class CriarContaCommandHandler : IRequestHandler<CriarContaCommand, ContaDto>
{
    private readonly IContaBancariaRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CriarContaCommandHandler(IContaBancariaRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ContaDto> Handle(CriarContaCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.DocumentoJaExisteAsync(request.DocumentoCliente))
        {
            throw new InvalidOperationException("Já existe uma conta cadastrada para este documento.");
        }

        var novaConta = new ContaBancaria(request.NomeCliente, request.DocumentoCliente);

        await _repository.AdicionarAsync(novaConta);

        await _unitOfWork.CompleteAsync(cancellationToken);

        return new ContaDto
        {
            Id = novaConta.Id,
            NomeCliente = novaConta.NomeCliente,
            DocumentoCliente = novaConta.DocumentoCliente,
            SaldoAtual = novaConta.Saldo,
            DataAbertura = novaConta.DataAbertura.ToString("o"),
            Status = novaConta.Status.ToString()
        };
    }
}