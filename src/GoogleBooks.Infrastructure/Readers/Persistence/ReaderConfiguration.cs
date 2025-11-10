using GoogleBooks.Domain.Readers.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace GoogleBooks.Infrastructure.Readers.Persistence;

public class ReaderConfiguration(IMongoDatabase database) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var collections = await database.ListCollectionNames(cancellationToken: cancellationToken).ToListAsync(cancellationToken);
        if (!collections.Contains(nameof(Reader)))
        {
            await database.CreateCollectionAsync(nameof(Reader), cancellationToken: cancellationToken);
        }

        var collection = database.GetCollection<Reader>(nameof(Reader));

        var emailIndex = Builders<Reader>.IndexKeys.Ascending(x => x.Email);
        await collection.Indexes.CreateOneAsync(new CreateIndexModel<Reader>(emailIndex, new CreateIndexOptions { Unique = true }), cancellationToken: cancellationToken);

        var compositeKey = Builders<Reader>.IndexKeys
            .Ascending(_ => _.City)
            .Ascending(_ => _.ZipCode)
            .Ascending(_ => _.Birthdate)
            .Ascending(_ => _.Id);

        await collection.Indexes.CreateOneAsync(
            new CreateIndexModel<Reader>(
                compositeKey,
                new CreateIndexOptions { Collation = ReaderListByCriteriaStrategy.GetStrategy() }),
            cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
