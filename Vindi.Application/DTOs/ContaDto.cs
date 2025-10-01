namespace Vindi.Application.DTOs;

public class ContaDto
{
    public Guid Id { get; set; }
    public string NomeCliente { get; set; }
    public string DocumentoCliente { get; set; }
    public decimal SaldoAtual { get; set; }
    public string DataAbertura { get; set; }
    public string Status { get; set; }
}