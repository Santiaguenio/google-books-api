using GoogleBooks.Infrastructure.Books.Mappers;
using GoogleBooks.Infrastructure.Books.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure;

public static class InfrastructureExtensions
{
    public static void RegisterInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        RegisterHttpClient(services, configuration);
        services.RegisterMappings();
        services.RegisterServices();
    }

    private static void RegisterHttpClient(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient(ServicesConstants.GoogleClientName)
            .ConfigureHttpClient(_ => _.BaseAddress = new(configuration.GetValue<string>("GoogleBooks:BaseUrl")!))
            .AddStandardResilienceHandler();
    }
}
