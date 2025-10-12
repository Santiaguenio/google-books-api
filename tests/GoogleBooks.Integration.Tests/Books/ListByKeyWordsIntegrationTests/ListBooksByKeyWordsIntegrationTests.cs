using GoogleBooks.Contracts;
using GoogleBooks.Contracts.Requests;
using GoogleBooks.Contracts.Responses.Books;
using GoogleBooks.Domain.Books;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoogleBooks.Integration.Tests.Books.ListByKeyWordsIntegrationTests;

[Collection("Integration tests")]
public class ListBooksByKeyWordsIntegrationTests(TestFactory testFactory) : IAsyncLifetime
{
    private readonly HttpClient _client = testFactory.CreateClient();

    async ValueTask IAsyncLifetime.InitializeAsync()
    {
        await Task.CompletedTask;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        testFactory.ResetMocks();

        GC.SuppressFinalize(this);
        await Task.CompletedTask;
    }

    [Theory]
    [MemberData(nameof(GetEntryDataAndExpectedResult))]
    public async Task Should(
        PageParams pageParams,
        StringContent expectedGoogleClientResponse,
        StringContent expectedResult)
    {
        // arrange
        testFactory.SetMockedHttpClientFactory(testFactory.GoogleBooksUrl);
        testFactory.MockedHttpMessageHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes?q={pageParams.KeyWords}&maxResults={pageParams.PageSize}&startIndex={(pageParams.Page - 1) * pageParams.PageSize}")),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = expectedGoogleClientResponse
            });

        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = expectedResult
        };

        // act
        var actualHttpResult = await _client.GetAsync($"books?keyWords={pageParams.KeyWords}&page={pageParams.Page}&pageSize={pageParams.PageSize}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(
            await expectedHttpResult.Content.ReadFromJsonAsync<EntitiesByCriteriaDto<BookFullDto>>(TestContext.Current.CancellationToken),
            await actualHttpResult.Content.ReadFromJsonAsync<EntitiesByCriteriaDto<BookFullDto>>(TestContext.Current.CancellationToken));

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == HttpMethod.Get && _.RequestUri!.AbsoluteUri.Equals($"{testFactory.GoogleBooksUrl}volumes?q={pageParams.KeyWords}&maxResults={pageParams.PageSize}&startIndex={(pageParams.Page - 1) * pageParams.PageSize}")),
                ItExpr.IsAny<CancellationToken>()
            );
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_KeyWordsIsNullOrWhiteSpace()
    {
        // arrange
        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.BadRequest);

        // act
        var actualHttpResult = await _client.GetAsync($"books?keyWords={string.Empty}", TestContext.Current.CancellationToken);

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
        // Google client response
        using var mockedListBooksByKeyWordsResponse = JsonDocument.Parse(File.ReadAllText("Books/ListByKeyWordsIntegrationTests/Should/MockedListBooksByKeyWordsResponse.json"));
        int mockedTotalItems = mockedListBooksByKeyWordsResponse.RootElement.GetProperty("totalItems").GetInt32();
        var mockedExpectedItems = mockedListBooksByKeyWordsResponse.RootElement.GetProperty("items");

        // Expected result
        using var listBooksByKeyWordsExpectedJsonResult = JsonDocument.Parse(File.ReadAllText("Books/ListByKeyWordsIntegrationTests/Should/ExpectedListBooksByKeyWordsResult.json"));
        int expectedTotalItems = listBooksByKeyWordsExpectedJsonResult.RootElement.GetProperty("totalItems").GetInt32();
        var expectedItemsResult = listBooksByKeyWordsExpectedJsonResult.RootElement.GetProperty("items");

        // 1 - First page with max page size
        var firstPage = 1;
        yield return new object[]
        {
            new PageParams { KeyWords = "federer", Page = firstPage * BookConstants.MaximalItemsPerPage, PageSize = BookConstants.MaximalItemsPerPage },

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = mockedTotalItems,
                    items =  mockedExpectedItems.EnumerateArray()
                        .Skip((firstPage - 1) * BookConstants.MaximalItemsPerPage)
                        .Take(BookConstants.MaximalItemsPerPage)
                })),

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = expectedTotalItems,
                    items =  expectedItemsResult.EnumerateArray()
                        .Skip((firstPage - 1) * BookConstants.MaximalItemsPerPage)
                        .Take(BookConstants.MaximalItemsPerPage)
                }))
        };

        // 2 - Seconds page with max page size
        var secondPage = 2;
        yield return new object[]
        {
            new PageParams { KeyWords = "federer", Page = secondPage, PageSize = BookConstants.MaximalItemsPerPage },

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = mockedTotalItems,
                    items =  mockedExpectedItems.EnumerateArray()
                        .Skip((secondPage - 1) * BookConstants.MaximalItemsPerPage)
                        .Take(BookConstants.MaximalItemsPerPage)
                })),

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = expectedTotalItems,
                    items =  expectedItemsResult.EnumerateArray()
                        .Skip((secondPage - 1) * BookConstants.MaximalItemsPerPage)
                        .Take(BookConstants.MaximalItemsPerPage)
                }))
        };

        // 3 - Final page with max page size -- NOT WORKING
        var thirdPage = 3;
        yield return new object[]
        {
            new PageParams { KeyWords = "federer", Page = thirdPage, PageSize = BookConstants.MaximalItemsPerPage },

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = mockedTotalItems,
                    items =  mockedExpectedItems.EnumerateArray()
                        .Skip((thirdPage - 1) * BookConstants.MaximalItemsPerPage)
                        .Take(BookConstants.MaximalItemsPerPage)
                })),

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = expectedTotalItems,
                    items =  expectedItemsResult.EnumerateArray()
                        .Skip((thirdPage - 1) * BookConstants.MaximalItemsPerPage)
                        .Take(BookConstants.MaximalItemsPerPage)
                }))
        };

        // 4 - In between page with smaller size
        var intermediatePage = 5;
        var pageSize = 2;
        yield return new object[]
        {
            new PageParams { KeyWords = "federer", Page = intermediatePage, PageSize = pageSize },

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = mockedTotalItems,
                    items =  mockedExpectedItems.EnumerateArray()
                        .Skip((intermediatePage - 1) * pageSize)
                        .Take(pageSize)
                })),

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = expectedTotalItems,
                    items =  expectedItemsResult.EnumerateArray()
                        .Skip((intermediatePage - 1) * pageSize)
                        .Take(pageSize)
                }))
        };
    }
}
