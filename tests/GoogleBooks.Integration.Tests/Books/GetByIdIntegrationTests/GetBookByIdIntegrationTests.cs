using GoogleBooks.Contracts.Responses;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace GoogleBooks.Integration.Tests.Books.GetByIdIntegrationTests;

[Collection("Integration tests")]
public class GetBookByIdIntegrationTests(TestFactory testFactory) : IAsyncLifetime
{
    private readonly HttpClient _client = testFactory.CreateClient();

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        testFactory.ResetMocks();
        await Task.CompletedTask;
    }

    async ValueTask IAsyncLifetime.InitializeAsync()
    {
        await Task.CompletedTask;
    }

    [Fact]
    public async Task Should()
    {
        // arrange
        var bookId = "s1gVAAAAYAAJ";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/Should/ExpectedGetBookByIdContent.json", TestContext.Current.CancellationToken))
        };

        testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
        testFactory.MockedHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/Should/MockedGetBookByIdResponse.json", TestContext.Current.CancellationToken))
            });

        // act
        var actualHttpResult = await _client.GetAsync($"books/{bookId}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken));

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_IdIsNullOrWhiteSpace()
    {
        // arrange
        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.BadRequest);

        // act
        var actualHttpResult = await _client.GetAsync($"books/{string.Empty}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == It.IsAny<HttpMethod>() && _.RequestUri!.AbsoluteUri.Equals(It.IsAny<string>())),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task Should_ReturnInternalServerError_When_ExternalServerErrorOccurs()
    {
        // arrange
        var bookId = "s1gVAAAAYAAJ";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/ExternalServerError/ExpectedExternalServerErrorResult.json", TestContext.Current.CancellationToken))
        };

        testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
        testFactory.MockedHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("This is a mocked exception message")
            });

        // act
        var actualHttpResult = await _client.GetAsync($"books/{bookId}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken));

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task Should_ReturnInternalServerError_When_ExternalServerTimeoutOccurs()
    {
        // arrange
        var bookId = "s1gVAAAAYAAJ";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/ExternalServerError/ExpectedExternalServerErrorResult.json", TestContext.Current.CancellationToken))
        };

        testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
        testFactory.MockedHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException());

        _client.Timeout = TimeSpan.FromMilliseconds(1500);

        // act
        var actualHttpResult = await _client.GetAsync($"books/{bookId}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken));

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task Should_ReturnInternalServerError_When_HttpClientThrowsException()
    {
        // arrange
        var bookId = "s1gVAAAAYAAJ";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/InternalServerError/ExpectedInternalServerErrorResult.json", TestContext.Current.CancellationToken))
        };

        testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
        testFactory.MockedHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new Exception("This is a mocked exception message"));

        // act
        var actualHttpResult = await _client.GetAsync($"books/{bookId}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken));

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task Should_ReturnServiceUnavailableError()
    {
        // arrange
        var unknownId = "unknownId";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/ServiceUnavailable/ExpectedServiceUnavailableContent.json", TestContext.Current.CancellationToken))
        };

        testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
        testFactory.MockedHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{unknownId}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/ServiceUnavailable/MockedUnavailableContent.json", TestContext.Current.CancellationToken))
            });

        // act
        var actualHttpResult = await _client.GetAsync($"books/{unknownId}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>(TestContext.Current.CancellationToken));

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{unknownId}")),
                ItExpr.IsAny<CancellationToken>()
            );
    }
}