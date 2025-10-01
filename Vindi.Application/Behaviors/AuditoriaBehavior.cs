using MediatR;
using Microsoft.Extensions.Logging;
using Vindi.Application.Interfaces;
using Vindi.Core.Entities;
using Vindi.Core.Interfaces;

namespace Vindi.Application.Behaviors;

public class AuditoriaBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IAuditavel
{
    private readonly IAuditoriaService _auditoriaService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AuditoriaBehavior<TRequest, TResponse>> _logger;

    public AuditoriaBehavior(IAuditoriaService auditoriaService, IUnitOfWork unitOfWork, ILogger<AuditoriaBehavior<TRequest, TResponse>> logger) 
    {
        _auditoriaService = auditoriaService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        _logger.LogInformation("--- Acionando Auditoria para o comando {CommandName} ---", typeof(TRequest).Name);

        const string usuarioResponsavel = "SISTEMA";
        string detalhes = request.ObterDetalhesAuditoria();
        string acao = typeof(TRequest).Name.Replace("Command", "");

        var log = new LogAuditoria(usuarioResponsavel, acao, detalhes);

        _auditoriaService.RegistrarLog(log);

        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("--- Auditoria para {CommandName} registrada com sucesso. ---", typeof(TRequest).Name);

        return response;
    }
}