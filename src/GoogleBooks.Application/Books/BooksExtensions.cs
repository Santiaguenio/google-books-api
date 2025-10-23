using GoogleBooks.Application.Books.UseCases;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Requests.Books;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Application.Books;

internal static class BooksExtensions
{
    internal static void RegisterBookDependencies(this IServiceCollection services)
    {
        services.AddScoped<IGetById<string>, GetBookById>();
        services.AddScoped<IListByCriteria<PageParams>, ListBooksByKeyWords>();
    }
}
