using GoogleBooks.Contracts.Requests;

namespace GoogleBooks.Application.Common.UseCases;

public interface ICreate<TRequest, TKey> where TRequest : IGoogleBooksRequest
{
    Task<TKey> DoAsync(TRequest request, CancellationToken cancellationToken);
}