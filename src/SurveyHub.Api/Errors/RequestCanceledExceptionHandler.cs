using Microsoft.AspNetCore.Diagnostics;

namespace SurveyHub.Api.Errors;

internal sealed class RequestCanceledExceptionHandler(ILogger<RequestCanceledExceptionHandler> logger) : IExceptionHandler
{
    private const int ClientClosedRequest = 499;

    public ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        if (exception is not OperationCanceledException || !context.RequestAborted.IsCancellationRequested)
            return ValueTask.FromResult(false);

        logger.LogInformation("Request was canceled by the client: {Method} {Path}", context.Request.Method, context.Request.Path);

        context.Response.StatusCode = ClientClosedRequest;
        return ValueTask.FromResult(true);
    }
}