using Bogus;
using GoogleBooks.Contracts.Common;
using GoogleBooks.Contracts.Readers.Requests;
using GoogleBooks.Contracts.Readers.Responses;
using GoogleBooks.Domain.Books;
using GoogleBooks.Domain.Readers.Entities;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace GoogleBooks.Integration.Tests.Readers.ListReadersByCriteriaIntegrationTests;

[Collection("Integration tests collection")]
public class ListReadersByCriteriaIntegrationTests(TestFactory testFactory) : IAsyncLifetime
{
    async ValueTask IAsyncLifetime.InitializeAsync()
    {
        await Task.CompletedTask;
    }

    async ValueTask IAsyncDisposable.DisposeAsync()
    {
        GC.SuppressFinalize(this);
        await testFactory.ClearDatabaseAsync();
    }

    [Theory]
    [MemberData(nameof(GetEntryDataAndExpectedResult))]
    public async Task Should(
        ReadersSearchCriteriaDto readersSearchCriteria,
        StringContent expectedResult)
    {
        // arrange
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
        var actualHttpResult = await factory.CreateClient().GetAsync(
            $"api/readers?city={readersSearchCriteria.City}" +
            $"&name={readersSearchCriteria.Name}" +
            $"&lastname={readersSearchCriteria.LastName}" +
            $"&zipcode={readersSearchCriteria.ZipCode}" +
            $"&birthdate={readersSearchCriteria.BirthDate}" +
            $"&page={readersSearchCriteria.Page}" +
            $"&pageSize={readersSearchCriteria.PageSize}",
            TestContext.Current.CancellationToken);

        // assert
        Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);
        Assert.Equivalent(
            await expectedHttpResult.Content.ReadFromJsonAsync<EntitiesByCriteriaDto<ReaderDto>>(TestContext.Current.CancellationToken),
            await actualHttpResult.Content.ReadFromJsonAsync<EntitiesByCriteriaDto<ReaderDto>>(TestContext.Current.CancellationToken));

        mockedLogger.Verify(_ =>
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
        // Expected result
        using var listBooksByKeyWordsExpectedJsonResult = JsonDocument.Parse(File.ReadAllText("Books/ListBooksByKeyWordsIntegrationTests/Should/ExpectedListBooksByKeyWordsResult.json"));
        int expectedTotalItems = listBooksByKeyWordsExpectedJsonResult.RootElement.GetProperty("totalItems").GetInt32();
        var expectedItemsResult = listBooksByKeyWordsExpectedJsonResult.RootElement.GetProperty("items");

        // 1 - First page with max page size
        var firstPage = 1;
        yield return new object[]
        {
            new Faker<Reader>().Generate(50),

            new ReadersSearchCriteriaDto { Page = firstPage, PageSize = BookConstants.MAXIMAL_ITEMS_PER_PAGE },

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
            new ReadersSearchCriteriaDto { Page = secondPage, PageSize = BookConstants.MAXIMAL_ITEMS_PER_PAGE },

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
            new ReadersSearchCriteriaDto { Page = thirdPage, PageSize = BookConstants.MAXIMAL_ITEMS_PER_PAGE },

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
            new ReadersSearchCriteriaDto { Page = intermediatePage, PageSize = pageSize },

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
