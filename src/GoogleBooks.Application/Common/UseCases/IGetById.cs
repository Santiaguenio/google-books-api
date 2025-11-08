using GoogleBooks.Contracts.Common;

namespace GoogleBooks.Application.Common.UseCases;

public interface IGetById<TKey>
{
    Task<IGoogleBooksResponse> DoAsync(TKey id, CancellationToken cancellationToken);
}
