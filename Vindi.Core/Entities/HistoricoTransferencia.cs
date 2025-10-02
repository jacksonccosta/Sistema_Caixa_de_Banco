using System;

namespace Vindi.Core.Entities;

public class HistoricoTransferencia
{
    public Guid Id { get; private set; }
    public DateTime Timestamp { get; private set; }
    public decimal Valor { get; private set; }

    public Guid ContaOrigemId { get; private set; }
    public string DocumentoOrigem { get; private set; }
    public ContaBancaria ContaOrigem { get; private set; }

    public Guid ContaDestinoId { get; private set; }
    public string DocumentoDestino { get; private set; }
    public ContaBancaria ContaDestino { get; private set; }

    private HistoricoTransferencia() { }

    public HistoricoTransferencia(ContaBancaria origem, ContaBancaria destino, decimal valor)
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
        Valor = valor;

        ContaOrigemId = origem.Id;
        DocumentoOrigem = origem.DocumentoCliente;

        ContaDestinoId = destino.Id;
        DocumentoDestino = destino.DocumentoCliente;
    }
}