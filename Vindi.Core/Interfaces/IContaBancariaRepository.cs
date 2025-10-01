using Vindi.Core.Entities;

namespace Vindi.Core.Interfaces;

/// <summary>
/// Define o contrato para o repositório de ContaBancaria,
/// desacoplando a lógica de negócio dos detalhes de acesso a dados.
/// </summary>
public interface IContaBancariaRepository
{
    /// <summary>
    /// Adiciona uma nova entidade ContaBancaria ao contexto.
    /// A persistência será feita pela Unit of Work.
    /// </summary>
    /// <param name="contaBancaria">A entidade a ser adicionada.</param>
    Task AdicionarAsync(ContaBancaria contaBancaria);

    /// <summary>
    /// Obtém uma entidade ContaBancaria pelo documento do cliente.
    /// </summary>
    /// <param name="documento">O documento a ser pesquisado.</param>
    /// <returns>A entidade ContaBancaria ou null se não for encontrada.</returns>
    Task<ContaBancaria?> ObterPorDocumentoAsync(string documento);

    /// <summary>
    /// Obtém uma entidade ContaBancaria pelo seu Id.
    /// </summary>
    /// <param name="id">O Guid da conta.</param>
    /// <returns>A entidade ContaBancaria ou null se não for encontrada.</returns>
    Task<ContaBancaria?> ObterPorIdAsync(Guid id);

    /// <summary>
    /// Verifica de forma otimizada se já existe uma conta para um determinado documento.
    /// </summary>
    /// <param name="documento">O documento a ser verificado.</param>
    /// <returns>True se o documento já existe, caso contrário, false.</returns>
    Task<bool> DocumentoJaExisteAsync(string documento);

    /// <summary>
    /// Lista as contas bancárias com base em filtros opcionais.
    /// </summary>
    /// <param name="nome">Filtro opcional por nome do cliente.</param>
    /// <param name="documento">Filtro opcional por documento do cliente.</param>
    /// <returns>Uma coleção de entidades ContaBancaria.</returns>
    Task<IEnumerable<ContaBancaria>> ListarContasAsync(string? nome, string? documento);
}