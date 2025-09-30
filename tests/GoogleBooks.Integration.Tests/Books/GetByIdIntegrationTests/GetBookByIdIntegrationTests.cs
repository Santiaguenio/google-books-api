using GoogleBooks.Contracts.Responses;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace GoogleBooks.Integration.Tests.Books.GetByIdIntegrationTests;

[Collection("Integration tests")]
public class GetBookByIdIntegrationTests
{
    private readonly TestFactory _testFactory;
    private readonly HttpClient _client;

    public GetBookByIdIntegrationTests(TestFactory testFactory)
    {
        _testFactory = testFactory;
        _client = _testFactory.CreateClient();
    }

    [Fact]
    public async Task Should()
    {
        // arrange
        var bookId = "s1gVAAAAYAAJ";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/Should/ExpectedGetBookByIdContent.json"))
        };

        _testFactory.SetMockedHttpClientFactory(_testFactory.GetGoogleBooksUrl);
        _testFactory.MockedHttpMessageHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{_testFactory.GetGoogleBooksUrl}volumes/{bookId}")),
                    ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.OK,
                   Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/Should/MockedGetBookByIdResponse.json"))
               });

        // act
        var actualHttpResult = await _client.GetAsync($"books/{bookId}");

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>());
    }

    [Fact]
    public async Task Should_ReturnInternalServerError_WhenExternalServerErrorOccurs()
    {
        // arrange
        var bookId = "s1gVAAAAYAAJ";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/ExternalServerError/ExpectedExternalServerErrorResult.json"))
        };

        _testFactory.SetMockedHttpClientFactory(_testFactory.GetGoogleBooksUrl);
        _testFactory.MockedHttpMessageHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{_testFactory.GetGoogleBooksUrl}volumes/{bookId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("This is a mocked exception message")
                });
        // act
        var actualHttpResult = await _client.GetAsync($"books/{bookId}");

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>());
    }

    [Fact]
    public async Task Should_ReturnInternalServerError_WhenExternalServerTimeoutOccurs()
    {
        // arrange
        var bookId = "s1gVAAAAYAAJ";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/ExternalServerError/ExpectedExternalServerErrorResult.json"))
        };

        _testFactory.SetMockedHttpClientFactory(_testFactory.GetGoogleBooksUrl);
        _testFactory.MockedHttpMessageHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{_testFactory.GetGoogleBooksUrl}volumes/{bookId}")),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException());

        _client.Timeout = TimeSpan.FromMilliseconds(1500);

        // act
        var actualHttpResult = await _client.GetAsync($"books/{bookId}");

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>());
    }

    [Fact]
    public async Task Should_ReturnInternalServerError_WhenHttpClientThrowsException()
    {
        // arrange
        var bookId = "s1gVAAAAYAAJ";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/InternalServerError/ExpectedInternalServerErrorResult.json"))
        };

        _testFactory.SetMockedHttpClientFactory(_testFactory.GetGoogleBooksUrl);
        _testFactory.MockedHttpMessageHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{_testFactory.GetGoogleBooksUrl}volumes/{bookId}")),
                    ItExpr.IsAny<CancellationToken>())
               .ThrowsAsync(new Exception("This is a mocked exception message"));

        // act
        var actualHttpResult = await _client.GetAsync($"books/{bookId}");

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>());
    }

    [Fact]
    public async Task Should_ReturnServiceUnavailableException()
    {
        // arrange
        var unknownId = "unknownId";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/ServiceUnavailable/ExpectedServiceUnavailableContent.json"))
        };

        _testFactory.SetMockedHttpClientFactory(_testFactory.GetGoogleBooksUrl);
        _testFactory.MockedHttpMessageHandler
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{_testFactory.GetGoogleBooksUrl}volumes/{unknownId}")),
                    ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(new HttpResponseMessage
               {
                   StatusCode = HttpStatusCode.ServiceUnavailable,
                   Content = new StringContent(await File.ReadAllTextAsync("Books/GetByIdIntegrationTests/ServiceUnavailable/MockedUnavailableContent.json"))
               });

        // act
        var actualHttpResult = await _client.GetAsync($"books/{unknownId}");

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BookFullDto>(), await actualHttpResult.Content.ReadFromJsonAsync<BookFullDto>());
    }
}