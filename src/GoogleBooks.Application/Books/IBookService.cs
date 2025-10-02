using GoogleBooks.Contracts.Requests;
using GoogleBooks.Contracts.Responses;

namespace GoogleBooks.Application.Books;

public interface IBookService
{
    Task<BookFullDto> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken);
    Task<BooksByKeyWordsDto> ListByKeyWordsAsync(PageParams request, CancellationToken cancellationToken);
}
