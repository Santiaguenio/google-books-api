using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GoogleBooks.WebApi.Middleware;

internal class ServiceUnavailableExceptionHandler(ILogger<ServiceUnavailableExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        if (exception is not HttpRequestException hre || hre.StatusCode != HttpStatusCode.ServiceUnavailable)
        {
            return false;
        }

        logger.LogError("[GoogleBooksApi Error] - EndPoint = {EndPoint}, Section = {Section}, ErrorMessage = {ErrorMessage}", $"{httpContext.Request} - Failure", exception.StackTrace, exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status503ServiceUnavailable,
            Title = "GoogleBooks service is temporarily unavailable"
        };

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}