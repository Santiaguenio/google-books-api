using GoogleBooks.Infrastructure.Books.Mappers;
using GoogleBooks.Infrastructure.Books.Services;
using GoogleBooks.Infrastructure.Common;
using GoogleBooks.Infrastructure.Common.Validators;
using GoogleBooks.Infrastructure.Readers;
using GoogleBooks.Infrastructure.Readers.Mappers;
using GoogleBooks.Infrastructure.Readers.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace GoogleBooks.Infrastructure;

public static class InfrastructureExtensions
{
    public static void RegisterInfrastructureServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterBookMappings();
        services.RegisterBookServices();

        services.RegisterCommonDependencies();

        services.RegisterReaderServices();
        services.RegisterReaderMappings();

        services.RegisterValidators();

        RegisterHttpClient(services, configuration);
        RegisterMongoDb(services, configuration);
    }

    private static void RegisterMongoDb(
        IServiceCollection services,
        IConfiguration configuration)
    {
        // Register MongoDB client and database in the DI container
        services.AddSingleton(_ => MongoDbConfiguration.CreateClient(configuration.GetConnectionString("MongoDb")!));

        services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(configuration.GetSection("MongoDbName").Value));

        services.AddHostedService<ReaderConfiguration>();
    }

    private static void RegisterHttpClient(
        IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHttpClient(ServicesConstants.GOOGLE_CLIENT_NAME)
            .ConfigureHttpClient(_ => _.BaseAddress = new(configuration.GetValue<string>("GoogleBooks:BaseUrl")!))
            .AddStandardResilienceHandler();
    }
}
