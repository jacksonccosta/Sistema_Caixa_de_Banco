using MediatR;
using Vindi.Application.DTOs;

namespace Vindi.Application.Commands;

public record CriarContaCommand(string NomeCliente, string DocumentoCliente) : IRequest<ContaDto>;