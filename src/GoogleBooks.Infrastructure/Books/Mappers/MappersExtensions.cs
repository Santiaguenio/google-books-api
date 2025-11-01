using GoogleBooks.Infrastructure.Books.Mappers.OutgoingMappings;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Books.Mappers;

internal static class MappersExtensions
{
    internal static void RegisterMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BookByIdProfile));
    }
}