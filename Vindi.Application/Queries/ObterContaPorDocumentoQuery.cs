using MediatR;
using Vindi.Application.DTOs;

namespace Vindi.Application.Queries;

public record ObterContaPorDocumentoQuery(string Documento) : IRequest<ContaDto?>;