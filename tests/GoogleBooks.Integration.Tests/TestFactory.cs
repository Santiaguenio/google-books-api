using GoogleBooks.Infrastructure.Books.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Testcontainers.MongoDb;

namespace GoogleBooks.Integration.Tests;

public class TestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoContainer = new MongoDbBuilder()
        .WithImage("mongo:8.0")
        .Build();

    private IMongoDatabase _database = default!;
    private readonly Mock<IHttpClientFactory> _mockedHttpClientFactory = new();
    private IServiceProvider _serviceProvider = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:MongoDb", _mongoContainer.GetConnectionString());

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IHttpClientFactory));
            services.AddSingleton(_mockedHttpClientFactory.Object);

            _serviceProvider = services.BuildServiceProvider();
            _database = _serviceProvider.GetRequiredService<IMongoDatabase>();

            ServiceProvider = services.BuildServiceProvider();
        });
    }

    internal string GoogleBooksUrl = "https://www.googleapis.com/books/v1/";

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
        _mockedHttpClientFactory.Reset();
        MockedHttpMessageHandler.Reset();
    }

    internal void SetMockedHttpClientFactory(string url = "https://default-test-url.com")
    {
        var httpClient = new HttpClient(MockedHttpMessageHandler.Object)
        {
            BaseAddress = new Uri(url)
        };

        _mockedHttpClientFactory
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
