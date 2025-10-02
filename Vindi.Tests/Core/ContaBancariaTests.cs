using Vindi.Core.Entities;
using Vindi.Core.Enums;
using Xunit;

namespace Vindi.Tests.Core;

public class ContaBancariaTests
{
    private const decimal SaldoInicial = 1000m;

    #region Constructor Tests

    [Fact]
    public void Construtor_DeveCriarContaComValoresIniciaisCorretos()
    {
        var nomeCliente = "Cliente Teste";
        var documentoCliente = "12345678901";

        var conta = new ContaBancaria(nomeCliente, documentoCliente);

        Assert.Equal(nomeCliente, conta.NomeCliente);
        Assert.Equal(documentoCliente, conta.DocumentoCliente);
        Assert.Equal(SaldoInicial, conta.Saldo);
        Assert.Equal(StatusConta.Ativa, conta.Status);
        Assert.True(conta.DataAbertura <= DateTime.UtcNow);
        Assert.Null(conta.DataInativacao);
    }

    [Theory]
    [InlineData(null, "12345678901")]
    [InlineData("Cliente Teste", null)]
    [InlineData("", "12345678901")]
    [InlineData("Cliente Teste", "")]
    public void Construtor_ComNomeOuDocumentoInvalidos_DeveLancarArgumentException(string nome, string documento)
    {
        Assert.Throws<ArgumentException>(() => new ContaBancaria(nome, documento));
    }

    #endregion

    #region Debitar Tests

    [Fact]
    public void Debitar_ComSaldoSuficiente_DeveSubtrairValorDoSaldo()
    {
        var conta = new ContaBancaria("Cliente Teste", "12345678901");
        var valorDebito = 100m;

        conta.Debitar(valorDebito);

        Assert.Equal(SaldoInicial - valorDebito, conta.Saldo);
    }

    [Fact]
    public void Debitar_ComSaldoInsuficiente_DeveLancarInvalidOperationException()
    {
        var conta = new ContaBancaria("Cliente Teste", "12345678901");
        var valorDebito = SaldoInicial + 1;

        Assert.Throws<InvalidOperationException>(() => conta.Debitar(valorDebito));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-100)]
    public void Debitar_ComValorNaoPositivo_DeveLancarArgumentException(decimal valorInvalido)
    {
        var conta = new ContaBancaria("Cliente Teste", "12345678901");

        Assert.Throws<ArgumentException>(() => conta.Debitar(valorInvalido));
    }

    [Fact]
    public void Debitar_ContaInativa_DeveLancarInvalidOperationException()
    {
        var conta = new ContaBancaria("Cliente Teste", "12345678901");
        conta.Inativar();

        Assert.Throws<InvalidOperationException>(() => conta.Debitar(100m));
    }

    #endregion

    #region Creditar Tests

    [Fact]
    public void Creditar_ValorValido_DeveAdicionarValorAoSaldo()
    {
        var conta = new ContaBancaria("Cliente Teste", "12345678901");
        var valorCredito = 200m;

        conta.Creditar(valorCredito);

        Assert.Equal(SaldoInicial + valorCredito, conta.Saldo);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-200)]
    public void Creditar_ComValorNaoPositivo_DeveLancarArgumentException(decimal valorInvalido)
    {
        var conta = new ContaBancaria("Cliente Teste", "12345678901");

        Assert.Throws<ArgumentException>(() => conta.Creditar(valorInvalido));
    }

    [Fact]
    public void Creditar_ContaInativa_DeveLancarInvalidOperationException()
    {
        var conta = new ContaBancaria("Cliente Teste", "12345678901");
        conta.Inativar();

        Assert.Throws<InvalidOperationException>(() => conta.Creditar(100m));
    }

    #endregion

    #region Inativar Tests

    [Fact]
    public void Inativar_ContaAtiva_DeveMudarStatusParaInativa()
    {
        var conta = new ContaBancaria("Cliente Teste", "12345678901");

        conta.Inativar();

        Assert.Equal(StatusConta.Inativa, conta.Status);
        Assert.NotNull(conta.DataInativacao);
    }

    [Fact]
    public void Inativar_ContaJaInativa_DeveLancarInvalidOperationException()
    {
        var conta = new ContaBancaria("Cliente Teste", "12345678901");
        conta.Inativar();

        Assert.Throws<InvalidOperationException>(() => conta.Inativar());
    }

    #endregion
}