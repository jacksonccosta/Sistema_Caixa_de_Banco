using MediatR;
using Vindi.Application.DTOs;

namespace Vindi.Application.Queries;

public record ListarContasQuery(string? Nome, string? Documento) : IRequest<IEnumerable<ContaDto>>;