using GoogleBooks.Infrastructure.Books.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;

namespace GoogleBooks.Integration.Tests;

public class TestFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IHttpClientFactory));
            services.AddSingleton(MockedHttpClientFactory.Object);
            services.AddSingleton(MockedHttpMessageHandler.Object);
        });
    }

    internal string GoogleBooksUrl = "https://www.googleapis.com/books/v1/";
    internal Mock<IHttpClientFactory> MockedHttpClientFactory { get; private set; } = new();
    internal Mock<HttpMessageHandler> MockedHttpMessageHandler { get; private set; } = new();

    internal void ResetMocks()
    {
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
}
