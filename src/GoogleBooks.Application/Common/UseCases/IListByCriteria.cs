using GoogleBooks.Contracts.Requests;
using GoogleBooks.Contracts.Responses;

namespace GoogleBooks.Application.Common.UseCases;

public interface IListByCriteria<TRequest> where TRequest : IGoogleBooksRequest
{
    Task<BooksByKeyWordsDto> DoAsync(TRequest request, CancellationToken cancellationToken);
}
