namespace GoogleBooks.WebApi.Middleware;

public abstract class ExceptionHandlerBase<TEntity>(ILogger<TEntity> logger)
{
    protected void Log(Exception exception, HttpContext httpContext)
    {
        logger.LogError("[GoogleBooksApi Error] - EndPoint = {EndPoint}, Section = {Section}, ErrorMessage = {ErrorMessage}", $"{httpContext.Request} - Failure", exception.StackTrace, exception.Message);
    }
}
