namespace GoogleBooks.Integration.Tests;

[Collection("Integration tests")]
public class GetBookByIdIntegrationTests : IAsyncLifetime
{
    private readonly TestFactory _testFactory;
    private readonly HttpClient _client;

    public GetBookByIdIntegrationTests(TestFactory testFactory)
    {
        _testFactory = testFactory;
        _client = _testFactory.CreateClient();
    }

    public Task DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public async Task Should()
    {
        // arrange

        // act

        // assert
    }
}