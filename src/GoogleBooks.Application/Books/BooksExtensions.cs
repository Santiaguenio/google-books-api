using GoogleBooks.Application.Books.UseCases;
using GoogleBooks.Application.Common.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Application.Books;

public static class BooksExtensions
{
    public static void RegisterBooksUseCases(this IServiceCollection services)
    {
        services.AddScoped<IGetById<string>, GetBookById>();
    }
}
