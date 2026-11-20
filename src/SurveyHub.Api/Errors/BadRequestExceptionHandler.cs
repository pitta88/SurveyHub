using Microsoft.AspNetCore.Diagnostics;

namespace SurveyHub.Api.Errors;

internal sealed class BadRequestExceptionHandler(
    ILogger<BadRequestExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is not BadHttpRequestException badRequest)
            return false;

        logger.LogWarning(exception, "Bad request: {Method} {Path}", context.Request.Method, context.Request.Path);

        context.Response.StatusCode = badRequest.StatusCode;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            Exception = exception,
            ProblemDetails =
            {
                Status = badRequest.StatusCode,
                Title = "The request is invalid.",
                Detail = "The request body or parameters could not be read."
            }
        });
    }
}