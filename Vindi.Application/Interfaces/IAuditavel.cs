namespace Vindi.Application.Interfaces;

/// <summary>
/// Interface marcadora para comandos que devem ser auditados.
/// Fornece um método para gerar a mensagem de log.
/// </summary>
public interface IAuditavel
{
    string ObterDetalhesAuditoria();
}