using GoogleBooks.Application.Books.UseCases;
using GoogleBooks.Application.Common.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Application.Books;

internal static class BooksExtensions
{
    internal static void RegisterBooksUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetById<string>, GetBookById>();
    }
}
