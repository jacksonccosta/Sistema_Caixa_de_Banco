using MediatR;
using Vindi.Application.DTOs;
using System.Collections.Generic;

namespace Vindi.Application.Queries;

public record ObterHistoricoTransferenciasQuery(string Documento) : IRequest<IEnumerable<TransferenciaDto>>;