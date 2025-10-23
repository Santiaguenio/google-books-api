using GoogleBooks.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GoogleBooks.WebApi.Middleware;

internal class EntityConflictExceptionHandler(ILogger<EntityConflictExceptionHandler> logger) : ExceptionHandlerBase<EntityConflictExceptionHandler>(logger), IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not EntityConflictException)
        {
            return false;
        }

        Log(exception, httpContext);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status409Conflict,
            Title = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}