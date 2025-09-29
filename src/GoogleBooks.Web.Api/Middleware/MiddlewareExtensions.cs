namespace GoogleBooks.WebApi.Middleware
{
    internal static class MiddlewareExtensions
    {
        internal static void RegisterExceptionHandlers(this IServiceCollection services)
        {
            services.AddExceptionHandler<ServiceUnavailableExceptionHandler>();
            services.AddExceptionHandler<ExternalServerExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }
    }
}
