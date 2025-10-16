using GoogleBooks.Application.Readers;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Readers.Services;

internal static class ServicesExtensions
{
    internal static void RegisterReaderServices(this IServiceCollection services)
    {
        services.AddScoped<IReaderService, ReaderService>();
    }
}
