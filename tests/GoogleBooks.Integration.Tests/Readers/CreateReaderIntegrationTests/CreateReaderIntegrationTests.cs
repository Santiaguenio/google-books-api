using GoogleBooks.Application.Readers;
using GoogleBooks.Contracts.Requests.Readers;
using GoogleBooks.Domain.Exceptions;
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

namespace GoogleBooks.Integration.Tests.Readers.CreateReaderIntegrationTests
{
    [Collection("Integration tests collection")]
    public class CreateReaderIntegrationTests(TestFactory testFactory) : IAsyncLifetime
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
            var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.Created);

            var address = "666 Evergreen Terrace";
            var birthDate = new DateOnly(1985, 10, 10);
            var city = "Springfield";
            var email = "johndoe@gmail.com";
            var name = "John";
            var lastName = "Doe";
            var zipCode = "65619";

            var readerCreationDto = new ReaderCreationDto
            {
                Address = address,
                Birthdate = birthDate,
                City = city,
                Email = email,
                Name = name,
                LastName = lastName,
                ZipCode = zipCode
            };

            var expectedCreatedReader = new Reader
            {
                Id = 1,
                Address = address,
                Birthdate = birthDate,
                City = city,
                Email = email,
                Name = name,
                LastName = lastName,
                ZipCode = zipCode
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
            var actualHttpResult = await factory.CreateClient().PostAsJsonAsync("api/readers", readerCreationDto, TestContext.Current.CancellationToken);

            // assert
            Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);

            var readerId = await actualHttpResult.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

            var actualCreatedReader = await _readerService.GetByIdAsync(readerId, TestContext.Current.CancellationToken);
            Assert.EquivalentWithExclusions(
                expectedCreatedReader,
                actualCreatedReader,
                _ => _.CreationDate,
                _ => _.LastUpdate);

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
        public async Task Should_ReturnConflictError_When_DuplicateEmail()
        {
            // arrange
            var existingReaderId = 1;
            var address = "666 Evergreen Terrace";
            var birthdate = new DateOnly(1985, 10, 10);
            var city = "Springfield";
            var email = "johndoe@gmail.com";
            var name = "John";
            var lastName = "Doe";
            var zipCode = "65619";

            await _readerService.AddAsync(new Reader
            {
                Id = existingReaderId,
                Address = address,
                Birthdate = birthdate,
                City = city,
                Email = email,
                Name = name,
                LastName = lastName,
                ZipCode = zipCode
            }, TestContext.Current.CancellationToken);

            var readerCreationDto = new ReaderCreationDto
            {
                Address = "another address",
                Birthdate = new DateOnly(1995, 10, 05),
                City = "another city",
                Email = email, // same email address
                Name = "Jane",
                LastName = "Doe",
                ZipCode = "91656"
            };

            var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                Content = JsonContent.Create(
                    new ProblemDetails
                    {
                        Title = "A write operation resulted in an error. WriteError: { Category : \"DuplicateKey\", Code : 11000, Message : \"E11000 duplicate key error collection: google-books.Reader index: email_1 dup key: { email: \"johndoe@gmail.com\" }\" }.",
                        Status = (int)HttpStatusCode.Conflict
                    }
                )
            };

            var mockedLogger = new Mock<ILogger<EntityConflictExceptionHandler>>();
            using var factory = testFactory.WithWebHostBuilder(_ =>
            {
                _.ConfigureTestServices(_ =>
                {
                    _.RemoveAll(typeof(ILogger<EntityConflictExceptionHandler>));
                    _.AddSingleton(mockedLogger.Object);
                });
            });

            // act
            var actualHttpResult = await factory.CreateClient().PostAsJsonAsync("api/readers", readerCreationDto, TestContext.Current.CancellationToken);

            // assert
            Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);

            Assert.Equivalent(
                await expectedHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken),
                await actualHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken));

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
