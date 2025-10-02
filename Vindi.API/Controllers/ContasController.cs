using MediatR;
using Microsoft.AspNetCore.Mvc;
using Vindi.Application.Commands;
using Vindi.Application.DTOs;
using Vindi.Application.Queries;

namespace Vindi.API.Controllers;

/// <summary>
/// Controlador responsável pelo gerenciamento de Contas Bancárias.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class ContasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Cria uma nova conta bancária.
    /// </summary>
    /// <remarks>
    /// Cria uma nova conta com um saldo inicial bônus de R$1000,00.
    /// O documento do cliente (CPF/CNPJ) deve ser único no sistema.
    /// </remarks>
    /// <param name="command">Dados para criação da conta (Nome e Documento do cliente).</param>
    /// <returns>Os dados da conta recém-criada.</returns>
    /// <response code="201">Retorna a conta recém-criada.</response>
    /// <response code="400">Se os dados fornecidos forem inválidos ou se o documento já existir.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ContaDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarConta([FromBody] CriarContaCommand command)
    {
        var contaDto = await _mediator.Send(command);
        return CreatedAtAction(nameof(ObterContaPorDocumento), new { documento = contaDto.DocumentoCliente }, contaDto);
    }

    /// <summary>
    /// Realiza uma transferência de valor entre duas contas.
    /// </summary>
    /// <remarks>
    /// Ambas as contas (origem e destino) devem estar ativas e a conta de origem deve possuir saldo suficiente.
    /// A operação é atômica: ou a transferência ocorre com sucesso, ou nada é alterado no banco de dados.
    /// </remarks>
    /// <param name="command">Dados da transferência (Documento de Origem, Documento de Destino e Valor).</param>
    /// <response code="204">Transferência realizada com sucesso.</response>
    /// <response code="400">Se o valor for inválido, se a conta de origem não tiver saldo ou se uma das contas estiver inativa.</response>
    /// <response code="404">Se a conta de origem ou destino não for encontrada.</response>
    [HttpPost("transferencia")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RealizarTransferencia([FromBody] RealizarTransferenciaCommand command)
    {
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Lista as contas cadastradas com opção de filtro.
    /// </summary>
    /// <remarks>
    /// Permite buscar contas por parte do nome do cliente ou pelo documento exato.
    /// Se nenhum filtro for fornecido, todas as contas serão retornadas.
    /// </remarks>
    /// <param name="nome">Filtra contas por parte do nome do cliente.</param>
    /// <param name="documento">Filtra contas pelo documento exato do cliente.</param>
    /// <returns>Uma lista de contas que correspondem aos filtros.</returns>
    /// <response code="200">Retorna a lista de contas.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ContaDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarContas([FromQuery] string? nome, [FromQuery] string? documento)
    {
        var query = new ListarContasQuery(nome, documento);
        var contas = await _mediator.Send(query);
        return Ok(contas);
    }

    /// <summary>
    /// Obtém os dados de uma conta específica pelo documento.
    /// </summary>
    /// <param name="documento">Documento (CPF/CNPJ) do titular da conta.</param>
    /// <returns>Os dados da conta encontrada.</returns>
    /// <response code="200">Retorna os dados da conta solicitada.</response>
    /// <response code="404">Se nenhuma conta for encontrada para o documento informado.</response>
    [HttpGet("{documento}")]
    [ProducesResponseType(typeof(ContaDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterContaPorDocumento(string documento)
    {
        var query = new ObterContaPorDocumentoQuery(documento);
        var conta = await _mediator.Send(query);

        return conta is not null ? Ok(conta) : NotFound(new { error = "Nenhuma conta encontrada para o documento informado." });
    }

    /// <summary>
    /// Obtém o histórico de transferências (enviadas e recebidas) de uma conta.
    /// </summary>
    /// <param name="documento">Documento do titular da conta.</param>
    /// <returns>Uma lista com o histórico de transferências.</returns>
    /// <response code="200">Retorna o histórico da conta.</response>
    /// <response code="404">Se nenhuma conta for encontrada para o documento informado.</response>
    [HttpGet("{documento}/historico-transferencias")]
    [ProducesResponseType(typeof(IEnumerable<TransferenciaDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterHistoricoTransferencias(string documento)
    {
        var query = new ObterHistoricoTransferenciasQuery(documento);
        var historico = await _mediator.Send(query);
        return Ok(historico);
    }

    /// <summary>
    /// Inativa uma conta bancária.
    /// </summary>
    /// <remarks>
    /// Altera o status de uma conta para "Inativa". Contas inativas não podem realizar ou receber transações.
    /// A ação é registrada para fins de auditoria (conforme requisito de negócio). Os dados históricos não são removidos.
    /// </remarks>
    /// <param name="documento">Documento do titular da conta a ser inativada.</param>
    /// <response code="204">Conta inativada com sucesso.</response>
    /// <response code="400">Se a conta já estiver inativa.</response>
    /// <response code="404">Se nenhuma conta for encontrada para o documento informado.</response>
    [HttpPatch("{documento}/inativar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> InativarConta(string documento)
    {
        var command = new InativarContaCommand(documento);
        await _mediator.Send(command);
        return NoContent();
    }
}