using GoogleBooks.Infrastructure.Books.Mappers.ExternalMappings;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Books.Mappers;

internal static class BookMapperExtensions
{
    internal static void RegisterBookMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BookByIdProfile));
    }
}