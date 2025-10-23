using GoogleBooks.Application.Readers;
using GoogleBooks.Domain.Exceptions;
using GoogleBooks.Domain.Readers.Entities;
using GoogleBooks.Infrastructure.Common;
using MongoDB.Driver;

namespace GoogleBooks.Infrastructure.Readers.Services;

internal class ReaderService(
    IdGeneratorHelper idGeneratorHelper,
    IMongoDatabase database) : IReaderService
{
    private readonly string _duplicatePattern = "duplicate key error";
    private readonly IMongoCollection<Reader> _collection = database.GetCollection<Reader>(nameof(Reader));

    public async Task<Reader> AddAsync(Reader reader, CancellationToken cancellationToken)
    {
        reader.Id = idGeneratorHelper.GetNextId(nameof(Reader));

        try
        {
            await _collection.InsertOneAsync(reader, cancellationToken: cancellationToken);
        }
        catch (MongoWriteException ex) when (ex.Message.Contains(_duplicatePattern, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new EntityConflictException(ex.Message);
        }

        return await GetByIdAsync(reader.Id, cancellationToken);
    }

    public async Task<Reader> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken)
    {
        return await (await _collection.FindAsync(_ => _.Id.Equals(id), cancellationToken: cancellationToken)).FirstOrDefaultAsync(cancellationToken);
    }
}
