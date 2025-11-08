using AutoMapper;
using GoogleBooks.Application.Common.Models;
using GoogleBooks.Application.Readers;
using GoogleBooks.Application.Readers.Models;
using GoogleBooks.Domain.Exceptions;
using GoogleBooks.Domain.Readers.Entities;
using GoogleBooks.Infrastructure.Common;
using MongoDB.Driver;

namespace GoogleBooks.Infrastructure.Readers.Services;

internal class ReaderService(
    IdGeneratorHelper idGeneratorHelper,
    IMongoDatabase database,
    IMapper mapper) : IReaderService
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

    public async Task<EntitiesByCriteria<ReaderFull>> ListByCriteriaAsync(ListByCriteriaParams request, CancellationToken cancellationToken)
    {
        var filterBuilder = Builders<Reader>.Filter;
        var filter = filterBuilder.And(
            request.City is not null ? filterBuilder.Eq(_ => _.City, request.City) : filterBuilder.Empty,
            request.Email is not null ? filterBuilder.Eq(_ => _.Email, request.Email) : filterBuilder.Empty,
            request.Name is not null ? filterBuilder.Eq(_ => _.Name, request.Name) : filterBuilder.Empty,
            request.LastName is not null ? filterBuilder.Eq(_ => _.LastName, request.LastName) : filterBuilder.Empty,
            request.ZipCode is not null ? filterBuilder.Eq(_ => _.ZipCode, request.ZipCode) : filterBuilder.Empty,
            request.BirthDate is not null ? filterBuilder.Eq(_ => _.Birthdate, request.BirthDate) : filterBuilder.Empty
        );

        var readers = (await (await _collection.FindAsync(filter, cancellationToken: cancellationToken))
            .ToListAsync(cancellationToken: cancellationToken))
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);

        return new EntitiesByCriteria<ReaderFull>
        {
            Items = mapper.Map<ReaderFull[]>(readers),
            TotalItems = readers.Count()
        };
    }
}
