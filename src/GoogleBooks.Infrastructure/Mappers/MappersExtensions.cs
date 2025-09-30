using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Mappers;

internal static class MappersExtensions
{
    internal static void RegisterMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(BookFullProfile));
    }
}