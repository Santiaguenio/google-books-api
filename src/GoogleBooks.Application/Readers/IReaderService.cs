using GoogleBooks.Application.Common.Models;
using GoogleBooks.Application.Common.Services;
using GoogleBooks.Application.Readers.Models;
using GoogleBooks.Domain.Readers.Entities;

namespace GoogleBooks.Application.Readers;

public interface IReaderService : IService<Reader>
{
    Task<Reader> AddAsync(Reader reader, CancellationToken cancellationToken);
    Task<EntitiesByCriteria<ReaderFull>> ListByCriteriaAsync(ListByCriteriaParams request, CancellationToken cancellationToken);
}
