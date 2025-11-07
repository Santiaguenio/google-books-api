using GoogleBooks.Contracts;
using GoogleBooks.Contracts.Requests.Books;
using GoogleBooks.Contracts.Responses.Books;
using GoogleBooks.Domain.Books;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoogleBooks.Integration.Tests.Books.ListBooksByKeyWordsIntegrationTests;

[Collection("Integration tests collection")]
public class ListBooksByKeyWordsIntegrationTests(TestFactory testFactory) : IAsyncLifetime
{
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
        BooksSearchCriteria pageParams,
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

        var mockedLogger = new Mock<ILogger>();
        using var factory = testFactory.WithWebHostBuilder(_ =>
        {
            _.ConfigureTestServices(_ =>
            {
                _.RemoveAll(typeof(ILogger));
                _.AddSingleton(mockedLogger.Object);
            });
        });

        // act
        var actualHttpResult = await factory.CreateClient().GetAsync($"api/books?keyWords={pageParams.KeyWords}&page={pageParams.Page}&pageSize={pageParams.PageSize}", TestContext.Current.CancellationToken);

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

        mockedLogger.Verify(_ =>
           _.Log(
               LogLevel.Error,
               It.IsAny<EventId>(),
               It.Is<It.IsAnyType>((state, _) => true),
               It.IsAny<Exception?>(),
               (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
           Times.Never);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_When_KeyWordsFieldsIsNullOrWhiteSpace()
    {
        // arrange
        var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.BadRequest);

        var mockedLogger = new Mock<ILogger>();
        using var factory = testFactory.WithWebHostBuilder(_ =>
        {
            _.ConfigureTestServices(_ =>
            {
                _.RemoveAll(typeof(ILogger));
                _.AddSingleton(mockedLogger.Object);
            });
        });

        // act
        var actualHttpResult = await factory.CreateClient().GetAsync($"api/books?keyWords={string.Empty}", TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);

        testFactory.MockedHttpMessageHandler
            .Protected().Verify(
                "SendAsync",
                Times.Never(),
                ItExpr.Is<HttpRequestMessage>(_ => _.Method == It.IsAny<HttpMethod>() && _.RequestUri!.AbsoluteUri.Equals(It.IsAny<string>())),
                ItExpr.IsAny<CancellationToken>()
            );

        mockedLogger.Verify(_ => // Handled by AspNet middleware
          _.Log(
              LogLevel.Error,
              It.IsAny<EventId>(),
              It.Is<It.IsAnyType>((state, _) => true),
              It.IsAny<Exception?>(),
              (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
          Times.Never);
    }

    public static IEnumerable<object[]> GetEntryDataAndExpectedResult()
    {
        // Google client response
        using var mockedListBooksByKeyWordsResponse = JsonDocument.Parse(File.ReadAllText("Books/ListBooksByKeyWordsIntegrationTests/Should/MockedListBooksByKeyWordsResponse.json"));
        int mockedTotalItems = mockedListBooksByKeyWordsResponse.RootElement.GetProperty("totalItems").GetInt32();
        var mockedExpectedItems = mockedListBooksByKeyWordsResponse.RootElement.GetProperty("items");

        // Expected result
        using var listBooksByKeyWordsExpectedJsonResult = JsonDocument.Parse(File.ReadAllText("Books/ListBooksByKeyWordsIntegrationTests/Should/ExpectedListBooksByKeyWordsResult.json"));
        int expectedTotalItems = listBooksByKeyWordsExpectedJsonResult.RootElement.GetProperty("totalItems").GetInt32();
        var expectedItemsResult = listBooksByKeyWordsExpectedJsonResult.RootElement.GetProperty("items");

        // 1 - First page with max page size
        var firstPage = 1;
        yield return new object[]
        {
            new BooksSearchCriteria { KeyWords = "federer", Page = firstPage, PageSize = BookConstants.MAXIMAL_ITEMS_PER_PAGE },

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = mockedTotalItems,
                    items =  mockedExpectedItems.EnumerateArray()
                        .Skip((firstPage - 1) * BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                        .Take(BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                })),

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = expectedTotalItems,
                    items =  expectedItemsResult.EnumerateArray()
                        .Skip((firstPage - 1) * BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                        .Take(BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                }))
        };

        // 2 - Seconds page with max page size
        var secondPage = 2;
        yield return new object[]
        {
            new BooksSearchCriteria { KeyWords = "federer", Page = secondPage, PageSize = BookConstants.MAXIMAL_ITEMS_PER_PAGE },

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = mockedTotalItems,
                    items =  mockedExpectedItems.EnumerateArray()
                        .Skip((secondPage - 1) * BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                        .Take(BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                })),

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = expectedTotalItems,
                    items =  expectedItemsResult.EnumerateArray()
                        .Skip((secondPage - 1) * BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                        .Take(BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                }))
        };

        // 3 - Final page with max page size
        var thirdPage = 3;
        yield return new object[]
        {
            new BooksSearchCriteria { KeyWords = "federer", Page = thirdPage, PageSize = BookConstants.MAXIMAL_ITEMS_PER_PAGE },

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = mockedTotalItems,
                    items =  mockedExpectedItems.EnumerateArray()
                        .Skip((thirdPage - 1) * BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                        .Take(BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                })),

            new StringContent(JsonSerializer.Serialize(
                new
                {
                    totalItems = expectedTotalItems,
                    items =  expectedItemsResult.EnumerateArray()
                        .Skip((thirdPage - 1) * BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                        .Take(BookConstants.MAXIMAL_ITEMS_PER_PAGE)
                }))
        };

        // 4 - In between page with smaller size
        var intermediatePage = 5;
        var pageSize = 2;
        yield return new object[]
        {
            new BooksSearchCriteria { KeyWords = "federer", Page = intermediatePage, PageSize = pageSize },

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
