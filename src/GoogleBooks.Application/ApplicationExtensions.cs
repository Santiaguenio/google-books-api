using GoogleBooks.Application.Books;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Application
{
    public static class ApplicationExtensions
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.RegisterBooksUseCases();
        }
    }
}
