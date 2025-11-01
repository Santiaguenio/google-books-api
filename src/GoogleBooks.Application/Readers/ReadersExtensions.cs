using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Application.Readers.UseCases;
using GoogleBooks.Contracts.Requests.Readers;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Application.Readers;

internal static class ReadersExtensions
{
    internal static void RegisterReaderDependencies(this IServiceCollection services)
    {
        RegisterUseCases(services);
    }

    private static void RegisterUseCases(IServiceCollection services)
    {
        services.AddScoped<ICreate<ReaderCreationDto, int>, CreateReader>();
        services.AddScoped<IGetById<int>, GetReaderById>();
    }
}
