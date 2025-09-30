using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GoogleBooks.WebApi.Middleware;

internal class ExternalServerExceptionHandler(ILogger<ExternalServerExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not HttpRequestException hre || hre.StatusCode != HttpStatusCode.InternalServerError)
        {
            return false;
        }

        logger.LogError("[GoogleBooksApi Error] - EndPoint = {EndPoint}, Section = {Section}, ErrorMessage = {ErrorMessage}", $"{httpContext.Request} - Failure", exception.StackTrace, exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "GoogleBooks service threw an exception, please try again later"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}