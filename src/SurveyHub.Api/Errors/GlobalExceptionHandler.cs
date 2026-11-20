using Microsoft.AspNetCore.Diagnostics;

namespace SurveyHub.Api.Errors;

internal sealed class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    IProblemDetailsService problemDetailsService) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        // 로그에는 자세히: 예외 전체 + 어떤 요청이었는지
        logger.LogError(exception, "Unhandled exception: {Method} {Path}",
            context.Request.Method, context.Request.Path);

        // 응답에는 최소한: 내부 정보 없이
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = context,
            Exception = exception,
            ProblemDetails =
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = "Please contact support with the trace ID if the problem persists."
            }
        });
    }
}