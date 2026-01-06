using GoogleBooks.Application.Readers;
using GoogleBooks.Contracts.Readers.Responses;
using GoogleBooks.Domain.Readers.Entities;
using GoogleBooks.WebApi.Middleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using System.Net.Http.Json;

namespace GoogleBooks.Integration.Tests.Readers.GetReaderByIdIntegrationTests
{
    [Collection("Integration tests collection")]
    public class GetReaderByIdIntegrationTests(TestFactory testFactory) : IAsyncLifetime
    {
        private readonly HttpClient _client = testFactory.CreateClient();
        private readonly IReaderService _readerService = testFactory.ServiceProvider.GetRequiredService<IReaderService>();

        async ValueTask IAsyncLifetime.InitializeAsync()
        {
            await Task.CompletedTask;
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            GC.SuppressFinalize(this);
            await testFactory.ClearDatabaseAsync();
        }

        [Fact]
        public async Task Should()
        {
            // arrange
            var expectedReaderId = 1;
            var address = "666 Evergreen Terrace";
            var birthdate = new DateOnly(1985, 10, 10);
            var city = "Springfield";
            var email = "johndoe@gmail.com";
            var name = "John";
            var lastName = "Doe";
            var zipCode = "65619";

            await _readerService.AddAsync(new Reader
            {
                Id = expectedReaderId,
                Address = address,
                Birthdate = birthdate,
                City = city,
                Email = email,
                Name = name,
                LastName = lastName,
                ZipCode = zipCode
            }, TestContext.Current.CancellationToken);

            var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(
                    new ReaderFullDto
                    {
                        Id = expectedReaderId,
                        Address = address,
                        Birthdate = birthdate,
                        City = city,
                        Email = email,
                        Name = name,
                        LastName = lastName,
                        ZipCode = zipCode
                    }
                )
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
            var actualHttpResult = await factory.CreateClient().GetAsync($"/api/readers/{expectedReaderId}", TestContext.Current.CancellationToken);

            // assert
            Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);

            Assert.Equal(
                await expectedHttpResult.Content.ReadFromJsonAsync<ReaderFullDto>(TestContext.Current.CancellationToken),
                await actualHttpResult.Content.ReadFromJsonAsync<ReaderFullDto>(TestContext.Current.CancellationToken));

            mockedLogger.Verify(_ =>
               _.Log(
                   It.IsAny<LogLevel>(),
                   It.IsAny<EventId>(),
                   It.Is<It.IsAnyType>((state, _) => true),
                   It.IsAny<Exception?>(),
                   (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
               Times.Never);
        }

        [Fact]
        public async Task Should_ReturnNotFoundError()
        {
            // arrange
            var unknownReaderId = 1;
            var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(await File.ReadAllTextAsync("Readers/GetReaderByIdIntegrationTests/ExpectedNotFoundErrorResult.json", TestContext.Current.CancellationToken))
            };

            var mockedLogger = new Mock<ILogger<EntityNotFoundExceptionHandler>>();
            using var factory = testFactory.WithWebHostBuilder(_ =>
            {
                _.ConfigureTestServices(_ =>
                {
                    _.RemoveAll(typeof(ILogger<EntityNotFoundExceptionHandler>));
                    _.AddSingleton(mockedLogger.Object);
                });
            });

            // act
            var actualHttpResult = await factory.CreateClient().GetAsync($"/api/readers/{unknownReaderId}", TestContext.Current.CancellationToken);

            // assert
            Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);

            var expectedResult = await expectedHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);
            var actualResult = await actualHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);
            Assert.Equivalent(expectedResult, actualResult);

            mockedLogger.Verify(_ =>
                _.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((state, _) => true),
                    It.IsAny<Exception?>(),
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);
        }
    }
}
