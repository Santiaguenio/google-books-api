using GoogleBooks.Application.Books;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Books.Services;

internal static class ServiceExtensions
{
    internal static void RegisterBookServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
    }
}
