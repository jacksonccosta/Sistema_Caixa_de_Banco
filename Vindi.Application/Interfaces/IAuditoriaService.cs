using Vindi.Core.Entities;

namespace Vindi.Application.Interfaces;

public interface IAuditoriaService
{
    void RegistrarLog(LogAuditoria log);
}