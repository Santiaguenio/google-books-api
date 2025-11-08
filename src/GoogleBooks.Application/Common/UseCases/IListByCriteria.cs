using GoogleBooks.Contracts.Books.Responses;
using GoogleBooks.Contracts.Common;

namespace GoogleBooks.Application.Common.UseCases;

public interface IListByCriteria<TRequest> where TRequest : IGoogleBooksRequest
{
    Task<IGoogleBooksResponse> DoAsync(TRequest request, CancellationToken cancellationToken);
}
