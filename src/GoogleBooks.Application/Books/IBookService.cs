using GoogleBooks.Contracts.Responses;

namespace GoogleBooks.Application.Books;

public interface IBookService
{
    Task<BookFullDto> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken);
}
