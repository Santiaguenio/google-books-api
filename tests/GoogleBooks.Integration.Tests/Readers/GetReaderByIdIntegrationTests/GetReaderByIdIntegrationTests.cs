using GoogleBooks.Application.Readers;
using GoogleBooks.Contracts.Responses.Readers;
using GoogleBooks.Domain.Readers.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

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
                Content = new StringContent(JsonSerializer.Serialize(new ReaderDto
                {
                    Id = expectedReaderId,
                    Address = address,
                    Birthdate = birthdate,
                    City = city,
                    Email = email,
                    Name = name,
                    LastName = lastName,
                    ZipCode = zipCode
                }))
            };

            // act
            var actualHttpResult = await _client.GetAsync($"/api/readers/{expectedReaderId}", TestContext.Current.CancellationToken);

            // assert
            Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);

            var expectedResult = await expectedHttpResult.Content.ReadFromJsonAsync<ReaderDto>(TestContext.Current.CancellationToken);
            var actualResult = await actualHttpResult.Content.ReadFromJsonAsync<ReaderDto>(TestContext.Current.CancellationToken);
            Assert.Equal(expectedResult, actualResult);
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

            // act
            var actualHttpResult = await _client.GetAsync($"/api/readers/{unknownReaderId}", TestContext.Current.CancellationToken);

            // assert
            Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);

            var expectedResult = await expectedHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);
            var actualResult = await actualHttpResult.Content.ReadFromJsonAsync<ProblemDetails>(TestContext.Current.CancellationToken);
            Assert.Equivalent(expectedResult, actualResult);
        }
    }
}
