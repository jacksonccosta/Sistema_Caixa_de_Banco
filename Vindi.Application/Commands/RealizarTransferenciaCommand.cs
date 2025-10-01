using MediatR;
using Vindi.Application.Interfaces;

namespace Vindi.Application.Commands;

public record RealizarTransferenciaCommand(string DocumentoOrigem, string DocumentoDestino, decimal Valor) : IRequest, IAuditavel
{
    public string ObterDetalhesAuditoria()
        => $"Transferência de {Valor:C} realizada da conta '{DocumentoOrigem}' para a conta '{DocumentoDestino}'.";
}