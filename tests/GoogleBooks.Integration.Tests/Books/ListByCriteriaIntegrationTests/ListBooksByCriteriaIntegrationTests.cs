using GoogleBooks.Contracts.Requests;
using GoogleBooks.Contracts.Responses;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;

namespace GoogleBooks.Integration.Tests.Books.ListByCriteriaIntegrationTests;

[Collection("Integration tests")]
public class ListBooksByCriteriaIntegrationTests(TestFactory testFactory) : IAsyncLifetime
{
    private readonly HttpClient _client = testFactory.CreateClient();

    public Task DisposeAsync()
    {
        testFactory.ResetMocks();
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    [Theory]
    [MemberData(nameof(GetEntryDataAndExpectedResult))]
    public async Task Should(PageParams pageParams)
    {
        // arrange
        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(await File.ReadAllTextAsync("Books/ListByCriteriaIntegrationTests/Should/ExpectedGetBooksByKeyWordContent.json"))
        };

        testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
        testFactory.MockedHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes?q={pageParams.KeyWords}&maxResults={pageParams.PageSize}&startIndex={pageParams.Page}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(await File.ReadAllTextAsync("Books/ListByCriteriaIntegrationTests/Should/MockedGetBooksByKeyWordResponse.json"))
            });

        // act
        var actualHttpResult = await _client.GetAsync($"books?keyWords={pageParams.KeyWords}&page={pageParams.Page}&pageSize={pageParams.PageSize}");

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(await expectedHttpResult.Content.ReadFromJsonAsync<BooksByKeyWordsDto>(), await actualHttpResult.Content.ReadFromJsonAsync<BooksByKeyWordsDto>());

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes?q={pageParams.KeyWords}&maxResults={pageParams.PageSize}&startIndex={pageParams.Page}")),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_KeyWordsIsNullOrWhiteSpace()
    {
        // arrange
        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.BadRequest);

        // act
        var actualHttpResult = await _client.GetAsync($"books?keyWords={string.Empty}");

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

    public static IEnumerable<object[]> GetEntryDataAndExpectedResult()
    {
        // 1 - Max page size and first page
        yield return new object[]
        {
            new PageParams { KeyWords = "federer", Page = 0, PageSize = 40 },

        };
    }
}
