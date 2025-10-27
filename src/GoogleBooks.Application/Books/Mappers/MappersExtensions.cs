using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Application.Books.Mappers;

internal static class MappersExtensions
{
    internal static void RegisterMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BookByIdProfile));
    }
}