using System.Net;
using GestaoColaboradores.Application.Common.Exceptions;

namespace GestaoColaboradores.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var (statusCode, title) = ex switch
            {
                NotFoundException => (HttpStatusCode.NotFound, "Recurso não encontrado"),
                ConflictException => (HttpStatusCode.Conflict, "Conflito de dados"),
                BusinessRuleException => (HttpStatusCode.BadRequest, "Regra de negócio violada"),
                UnauthorizedException => (HttpStatusCode.Unauthorized, "Não autorizado"),
                _ => (HttpStatusCode.InternalServerError, "Erro interno do servidor"),
            };

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.LogError(ex, "Erro não tratado ao processar {Path}", context.Request.Path);
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsJsonAsync(new
            {
                title,
                status = (int)statusCode,
                detail = ex.Message,
            });
        }
    }
}
