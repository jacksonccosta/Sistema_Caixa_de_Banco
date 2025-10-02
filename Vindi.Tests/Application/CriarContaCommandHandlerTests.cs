using Moq;
using Vindi.Application.Commands;
using Vindi.Application.Handlers;
using Vindi.Core.Entities;
using Vindi.Core.Interfaces;
using Xunit;

namespace Vindi.Tests.Application;

public class CriarContaCommandHandlerTests
{
    private readonly Mock<IContaBancariaRepository> _mockContaRepo;
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly CriarContaCommandHandler _handler;

    public CriarContaCommandHandlerTests()
    {
        _mockContaRepo = new Mock<IContaBancariaRepository>();
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _handler = new CriarContaCommandHandler(_mockContaRepo.Object, _mockUnitOfWork.Object);
    }

    [Fact]
    public async Task Handle_DocumentoNaoExistente_DeveCriarContaEChamarUnitOfWork()
    {
        var command = new CriarContaCommand("Cliente Novo", "11223344556");
        _mockContaRepo.Setup(r => r.DocumentoJaExisteAsync(command.DocumentoCliente)).ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        _mockContaRepo.Verify(r => r.AdicionarAsync(It.Is<ContaBancaria>(c => c.DocumentoCliente == command.DocumentoCliente)), Times.Once);

        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(CancellationToken.None), Times.Once);

        Assert.NotNull(result);
        Assert.Equal(command.NomeCliente, result.NomeCliente);
    }

    [Fact]
    public async Task Handle_DocumentoJaExistente_DeveLancarInvalidOperationException()
    {
        var command = new CriarContaCommand("Cliente Repetido", "11223344556");
        _mockContaRepo.Setup(r => r.DocumentoJaExisteAsync(command.DocumentoCliente)).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));

        _mockUnitOfWork.Verify(uow => uow.CompleteAsync(CancellationToken.None), Times.Never);
    }
}