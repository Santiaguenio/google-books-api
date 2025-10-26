using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Common;

internal static class CommonExtensions
{
    internal static void RegisterCommonDependencies(this IServiceCollection services)
    {
        services.AddScoped<IdGeneratorHelper>();
    }
}
