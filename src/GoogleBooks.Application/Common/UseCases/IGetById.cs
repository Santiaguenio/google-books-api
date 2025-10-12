using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Application.Common.UseCases;

public interface IGetById<TKey>
    where TKey : class
{
    Task<IGoogleBooksResponse> DoAsync(TKey id, CancellationToken cancellationToken);
}
