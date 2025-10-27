using GoogleBooks.Application.Common.Mappers;
using GoogleBooks.Application.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Application.Common;

internal static class CommonExtensions
{
    internal static void RegisterCommonDependencies(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.RegisterMappings();
    }
}
