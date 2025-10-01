namespace Vindi.Core.Entities;

public class LogAuditoria
{
    public Guid Id { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string UsuarioResponsavel { get; private set; }
    public string Acao { get; private set; }
    public string Detalhes { get; private set; }

    private LogAuditoria() { }

    public LogAuditoria(string usuarioResponsavel, string acao, string detalhes)
    {
        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
        UsuarioResponsavel = usuarioResponsavel;
        Acao = acao;
        Detalhes = detalhes;
    }
}