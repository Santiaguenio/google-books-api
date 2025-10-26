using GoogleBooks.Infrastructure.Books.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Testcontainers.MongoDb;
using MongoDbConfiguration = GoogleBooks.Infrastructure.MongoDbConfiguration;

namespace GoogleBooks.Integration.Tests;

public class TestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:8.0")
        .Build();

    private IMongoDatabase _database = default!;
    private IServiceProvider _serviceProvider = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IHttpClientFactory));
            services.AddSingleton(MockedHttpClientFactory.Object);
            services.AddSingleton(MockedHttpMessageHandler.Object);

            services.RemoveAll(typeof(IMongoDatabase));
            services.AddSingleton(_ => MongoDbConfiguration.CreateClient(_mongoContainer.GetConnectionString()!));
            services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase("google-books"));

            _serviceProvider = services.BuildServiceProvider();
            _database = _serviceProvider.GetRequiredService<IMongoDatabase>();

            ServiceProvider = services.BuildServiceProvider();
        });
    }

    internal string GoogleBooksUrl = "https://www.googleapis.com/books/v1/";

    internal Mock<IHttpClientFactory> MockedHttpClientFactory { get; private set; } = new();
    internal Mock<HttpMessageHandler> MockedHttpMessageHandler { get; private set; } = new();

    internal IServiceProvider ServiceProvider = default!;

    public async Task ClearDatabaseAsync()
    {
        var collections = await _database.ListCollectionNamesAsync();
        foreach (var name in await collections.ToListAsync())
        {
            var collection = _database.GetCollection<BsonDocument>(name);
            await collection.DeleteManyAsync(FilterDefinition<BsonDocument>.Empty);
        }
    }

    internal void ResetMocks()
    {
        MockedHttpClientFactory.Reset();
        MockedHttpMessageHandler.Reset();
    }

    internal void SetMockedHttpClientFactory(string url = "https://default-test-url.com")
    {
        var httpClient = new HttpClient(MockedHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(url)
        };

        MockedHttpClientFactory
            .Setup(_ => _.CreateClient(ServicesConstants.GOOGLE_CLIENT_NAME))
            .Returns(httpClient);
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        await _mongoContainer.DisposeAsync().AsTask();
        GC.SuppressFinalize(this);
    }

    public async ValueTask InitializeAsync()
    {
        await _mongoContainer.StartAsync();
    }
}
