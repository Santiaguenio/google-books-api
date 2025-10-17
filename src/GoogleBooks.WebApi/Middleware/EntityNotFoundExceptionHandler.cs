using GoogleBooks.Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GoogleBooks.WebApi.Middleware;

public class EntityNotFoundExceptionHandler(ILogger<EntityNotFoundExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not EntityNotFoundException)
        {
            return false;
        }

        logger.LogError("[GoogleBooksApi Error] - EndPoint = {EndPoint}, Section = {Section}, ErrorMessage = {ErrorMessage}", $"{httpContext.Request} - Failure", exception.StackTrace, exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status404NotFound,
            Title = exception.Message
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
