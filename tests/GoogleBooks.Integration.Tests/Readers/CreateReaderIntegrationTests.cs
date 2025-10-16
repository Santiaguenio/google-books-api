using GoogleBooks.Application.Readers;
using GoogleBooks.Contracts.Requests.Readers;
using GoogleBooks.Domain.Readers.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Http.Json;

namespace GoogleBooks.Integration.Tests.Readers
{
    [Collection("Integration tests")]
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
            await Task.CompletedTask;
        }

        [Fact]
        public async Task Should()
        {
            // arrange
            var expectedHttpResult = new HttpResponseMessage(HttpStatusCode.Created);

            var address = "666 Evergreen Terrace";
            var birthDate = new DateTime(1990, 10, 15, 0, 0, 0, DateTimeKind.Utc);
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

            // act
            var actualHttpResult = await _client.PostAsync("api/readers", JsonContent.Create(readerCreationDto), TestContext.Current.CancellationToken);

            // assert
            Assert.Equal(expectedHttpResult.StatusCode, actualHttpResult.StatusCode);

            var readerId = await actualHttpResult.Content.ReadFromJsonAsync<int>(TestContext.Current.CancellationToken);

            var actualCreatedReader = await _readerService.GetByIdAsync(readerId, TestContext.Current.CancellationToken);
            Assert.EquivalentWithExclusions(
                expectedCreatedReader,
                actualCreatedReader,
                _ => _.CreationDate,
                _ => _.LastUpdate);
        }
    }
}
