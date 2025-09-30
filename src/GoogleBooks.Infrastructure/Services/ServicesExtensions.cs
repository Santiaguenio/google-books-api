using GoogleBooks.Application.Books;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Services;

internal static class ServicesExtensions
{
    internal static void RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IBookService, BookService>();
    }
}
