using MediatR;
using Vindi.Application.Interfaces;

namespace Vindi.Application.Commands;

public record InativarContaCommand(string DocumentoCliente) : IRequest, IAuditavel
{
    public string ObterDetalhesAuditoria() => $"Conta com documento '{DocumentoCliente}' foi inativada.";
}