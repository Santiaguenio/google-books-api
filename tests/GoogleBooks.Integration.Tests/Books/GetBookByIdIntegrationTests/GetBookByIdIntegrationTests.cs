using GoogleBooks.Contracts.Responses.Books;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace GoogleBooks.Integration.Tests.Books.GetBookByIdIntegrationTests;

[Collection("Integration tests collection")]
public class GetBookByIdIntegrationTests(TestFactory testFactory) : IAsyncLifetime
{
    private readonly HttpClient _client = testFactory.CreateClient();

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        testFactory.ResetMocks();
        GC.SuppressFinalize(this);

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
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetBookByIdIntegrationTests/Should/ExpectedGetBookByIdContent.json", TestContext.Current.CancellationToken))
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
                Content = new StringContent(await File.ReadAllTextAsync("Books/GetBookByIdIntegrationTests/Should/MockedGetBookByIdResponse.json", TestContext.Current.CancellationToken))
            });

        // act
        var actualHttpResult = await _client.GetAsync($"api/books/{bookId}", TestContext.Current.CancellationToken);

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
        var actualHttpResult = await _client.GetAsync($"api/books/{string.Empty}", TestContext.Current.CancellationToken);

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
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetBookByIdIntegrationTests/ExpectedExternalServerErrorResult.json", TestContext.Current.CancellationToken))
        };

        testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
        testFactory.MockedHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException(
                "This is an external server error",
                new Exception("This is an external server error"),
                HttpStatusCode.InternalServerError));

        // act
        var actualHttpResult = await _client.GetAsync($"api/books/{bookId}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken), await actualHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken));

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    //[Fact]
    //public async Task Should_ReturnInternalServerError_When_ExternalServerTimeoutOccurs()
    //{
    //    // arrange
    //    var bookId = "s1gVAAAAYAAJ";

    //    var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.InternalServerError)
    //    {
    //        Content = new StringContent(await File.ReadAllTextAsync("Books/GetBookByIdIntegrationTests/ExpectedExternalServerErrorResult.json", TestContext.Current.CancellationToken))
    //    };

    //    testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
    //    testFactory.MockedHttpMessageHandler
    //        .Protected()
    //        .Setup<Task<HttpResponseMessage>>(
    //            "SendAsync",
    //            ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
    //            ItExpr.IsAny<CancellationToken>())
    //        .ThrowsAsync(new TaskCanceledException());

    //    // act
    //    var actualHttpResult = await _client.GetAsync($"api/books/{bookId}", TestContext.Current.CancellationToken);

    //    // assert
    //    Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
    //    Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken), await actualHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken));

    //    testFactory.MockedHttpMessageHandler
    //        .Protected().Verify(
    //            "SendAsync",
    //            Times.Once(),
    //            ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{bookId}")),
    //            ItExpr.IsAny<CancellationToken>()
    //        );
    //}

    [Fact]
    public async Task Should_ReturnInternalServerError_When_HttpClientThrowsException()
    {
        // arrange
        var bookId = "s1gVAAAAYAAJ";

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(await File.ReadAllTextAsync("ExpectedInternalServerErrorResult.json", TestContext.Current.CancellationToken))
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
        var actualHttpResult = await _client.GetAsync($"api/books/{bookId}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken), await actualHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken));

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
            Content = new StringContent(await File.ReadAllTextAsync("Books/GetBookByIdIntegrationTests/ServiceUnavailable/ExpectedServiceUnavailableContent.json", TestContext.Current.CancellationToken))
        };

        testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
        testFactory.MockedHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{unknownId}")),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException(
                await File.ReadAllTextAsync("Books/GetBookByIdIntegrationTests/ServiceUnavailable/MockedUnavailableContent.json", TestContext.Current.CancellationToken),
                new Exception("Response status code does not indicate success: 503 (Service Unavailable)."),
                HttpStatusCode.ServiceUnavailable));

        // act
        var actualHttpResult = await _client.GetAsync($"api/books/{unknownId}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken), await actualHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken));

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes/{unknownId}")),
                ItExpr.IsAny<CancellationToken>()
            );
    }
}