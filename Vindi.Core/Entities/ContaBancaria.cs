using Vindi.Core.Enums;

namespace Vindi.Core.Entities;

public class ContaBancaria
{
    private const decimal SaldoInicialBonus = 1000m;

    public Guid Id { get; private set; }
    public string NomeCliente { get; private set; }
    public string DocumentoCliente { get; private set; }
    public decimal Saldo { get; private set; }
    public DateTime DataAbertura { get; private set; }
    public StatusConta Status { get; private set; }
    public DateTime? DataInativacao { get; private set; }

    private ContaBancaria() { }

    public ContaBancaria(string nomeCliente, string documentoCliente)
    {
        if (string.IsNullOrWhiteSpace(nomeCliente))
            throw new ArgumentException("O nome do cliente é obrigatório.", nameof(nomeCliente));

        if (string.IsNullOrWhiteSpace(documentoCliente))
            throw new ArgumentException("O documento do cliente é obrigatório.", nameof(documentoCliente));

        Id = Guid.NewGuid();
        NomeCliente = nomeCliente;
        DocumentoCliente = documentoCliente;
        Saldo = SaldoInicialBonus;
        DataAbertura = DateTime.UtcNow;
        Status = StatusConta.Ativa;
    }

    public void Debitar(decimal valor)
    {
        if (Status == StatusConta.Inativa)
            throw new InvalidOperationException("A conta de origem está inativa.");

        if (valor <= 0)
            throw new ArgumentException("O valor da transferência deve ser positivo.", nameof(valor));

        if (Saldo < valor)
            throw new InvalidOperationException("Saldo insuficiente para a transferência.");

        Saldo -= valor;
    }

    public void Creditar(decimal valor)
    {
        if (Status == StatusConta.Inativa)
            throw new InvalidOperationException("A conta de destino está inativa.");

        if (valor <= 0)
            throw new ArgumentException("O valor do crédito deve ser positivo.", nameof(valor));

        Saldo += valor;
    }

    public void Inativar()
    {
        if (Status == StatusConta.Inativa)
            throw new InvalidOperationException("A conta já está inativa.");

        Status = StatusConta.Inativa;
        DataInativacao = DateTime.UtcNow;
    }
}