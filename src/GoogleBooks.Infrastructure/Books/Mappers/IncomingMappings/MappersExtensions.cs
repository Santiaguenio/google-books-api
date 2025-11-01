using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Books.Mappers.IncomingMappings;

internal static class MappersExtensions
{
    internal static void RegisterBookMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BookByIdProfile));
    }
}