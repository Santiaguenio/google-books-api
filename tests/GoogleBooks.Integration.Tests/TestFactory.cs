using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;

namespace GoogleBooks.Integration.Tests;

public class TestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IHttpClientFactory));
            services.AddSingleton(MockedHttpClientFactory.Object);

        });
    }
    
    internal Mock<IHttpClientFactory> MockedHttpClientFactory { get; private set; } = new();

    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        throw new NotImplementedException();
    }
}
