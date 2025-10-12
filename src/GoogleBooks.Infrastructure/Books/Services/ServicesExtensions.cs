using GoogleBooks.Application.Books;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Books.Services;

internal static class ServicesExtensions
{
    internal static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
    }
}
