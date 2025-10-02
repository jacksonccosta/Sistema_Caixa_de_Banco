using Moq;
using Vindi.Application.Commands;
using Vindi.Application.Handlers;
using Vindi.Core.Entities;
using Vindi.Core.Interfaces;
using Xunit;

namespace Vindi.Tests.Application;

public class RealizarTransferenciaCommandHandlerTests
{
    private readonly Mock<IContaBancariaRepository> _mockContaRepo;
    private readonly Mock<IHistoricoTransferenciaRepository> _mockHistoricoRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly RealizarTransferenciaCommandHandler _handler;

    public RealizarTransferenciaCommandHandlerTests()
    {
        _mockContaRepo = new Mock<IContaBancariaRepository>();
        _mockHistoricoRepo = new Mock<IHistoricoTransferenciaRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new RealizarTransferenciaCommandHandler(_mockContaRepo.Object, _mockHistoricoRepo.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_ContasValidasESaldoSuficiente_DeveRealizarTransferenciaEChamarUnitOfWork()
    {
        var contaOrigem = new ContaBancaria("Cliente Origem", "123");
        var contaDestino = new ContaBancaria("Cliente Destino", "456");
        var command = new RealizarTransferenciaCommand("123", "456", 100m);

        _mockContaRepo.Setup(r => r.ObterPorDocumentoAsync("123")).ReturnsAsync(contaOrigem);
        _mockContaRepo.Setup(r => r.ObterPorDocumentoAsync("456")).ReturnsAsync(contaDestino);

        await _handler.Handle(command, CancellationToken.None);

        Assert.Equal(900m, contaOrigem.Saldo);
        Assert.Equal(1100m, contaDestino.Saldo);
        _mockHistoricoRepo.Verify(r => r.AdicionarAsync(It.IsAny<HistoricoTransferencia>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task Handle_ContaOrigemNaoEncontrada_DeveLancarKeyNotFoundException()
    {
        var command = new RealizarTransferenciaCommand("123", "456", 100m);
        _mockContaRepo.Setup(r => r.ObterPorDocumentoAsync("123")).ReturnsAsync((ContaBancaria?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}