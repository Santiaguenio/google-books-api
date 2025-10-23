using GoogleBooks.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace GoogleBooks.WebApi.Middleware;

public class BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger) : ExceptionHandlerBase<BadRequestExceptionHandler>(logger), IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BadRequestException)
        {
            return false;
        }

        Log(exception, httpContext);

        var problemDetails = new BadRequestExceptionResponse(StatusCodes.Status400BadRequest, ((BadRequestException)exception).Errors!);

        httpContext.Response.StatusCode = problemDetails.Status;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
