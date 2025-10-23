using GoogleBooks.Domain.Readers.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace GoogleBooks.Infrastructure.Readers;

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
        var birthdayIndex = Builders<Reader>.IndexKeys.Ascending(x => x.Birthdate);
        await collection.Indexes.CreateOneAsync(new CreateIndexModel<Reader>(birthdayIndex), cancellationToken: cancellationToken);

        var keys = Builders<Reader>.IndexKeys.Ascending(x => x.Email);
        await collection.Indexes.CreateOneAsync(new CreateIndexModel<Reader>(keys, new CreateIndexOptions { Unique = true }), cancellationToken: cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
