using GoogleBooks.Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Common.Mappers;

internal static class MapperExtensions
{
    internal static void RegisterMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(EntitiesByCriteriaProfile));

        services.AddScoped<IGoogleBooksMapper, GoogleBooksMapper>();
    }
}
