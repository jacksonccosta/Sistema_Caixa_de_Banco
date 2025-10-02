using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vindi.Core.Entities;

namespace Vindi.Core.Interfaces;

public interface IHistoricoTransferenciaRepository
{
    Task AdicionarAsync(HistoricoTransferencia historico);
    Task<IEnumerable<HistoricoTransferencia>> ObterPorDocumentoAsync(string documento);
}