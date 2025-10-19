namespace GoogleBooks.WebApi.Middleware
{
    internal static class MiddlewareExtensions
    {
        internal static void RegisterExceptionHandlers(this IServiceCollection services)
        {
            services.AddExceptionHandler<EntityNotFoundExceptionHandler>();
            services.AddExceptionHandler<EntityConflictExceptionHandler>();
            services.AddExceptionHandler<ExternalServerExceptionHandler>();
            services.AddExceptionHandler<ServiceUnavailableExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
        }
    }
}
