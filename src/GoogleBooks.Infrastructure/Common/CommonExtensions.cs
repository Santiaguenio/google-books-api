using GoogleBooks.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Books.Services;

internal static class CommonExtensions
{
    internal static void RegisterCommonDependencies(this IServiceCollection services)
    {
        services.AddScoped<IdGeneratorHelper>();
    }
}
