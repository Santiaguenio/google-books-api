using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GoogleBooks.WebApi.Middleware;

public class ServiceUnavailableExceptionHandler(ILogger<ServiceUnavailableExceptionHandler> logger) : ExceptionHandlerBase<ServiceUnavailableExceptionHandler>(logger), IExceptionHandler
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

        Log(exception, httpContext);

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