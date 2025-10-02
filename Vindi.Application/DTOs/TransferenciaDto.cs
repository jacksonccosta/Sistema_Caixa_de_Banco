namespace Vindi.Application.DTOs;

public class TransferenciaDto
{
    public Guid Id { get; set; }
    public string Timestamp { get; set; }
    public decimal Valor { get; set; }
    public string DocumentoOrigem { get; set; }
    public string DocumentoDestino { get; set; }
    public string Tipo { get; set; }
}