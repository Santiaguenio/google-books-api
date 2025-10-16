using GoogleBooks.Application.Common.Services;
using GoogleBooks.Domain.Readers.Entities;

namespace GoogleBooks.Application.Readers;

public interface IReaderService : IService<Reader>
{
    Task<Reader> AddAsync(Reader reader, CancellationToken cancellationToken);
}
