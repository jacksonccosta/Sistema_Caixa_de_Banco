namespace Vindi.Core.Interfaces;

/// <summary>
/// Representa o padrão Unit of Work, centralizando a responsabilidade de salvar
/// todas as alterações de uma transação de negócio no banco de dados.
/// </summary>
public interface IUnitOfWork
{
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);
}