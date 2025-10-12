using GoogleBooks.Contracts;
using GoogleBooks.Contracts.Requests;
using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Application.Books;

public interface IBookService
{
    Task<BookFullDto> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken);
    Task<EntitiesByCriteriaDto<BookFullDto>> ListByKeyWordsAsync(PageParams request, CancellationToken cancellationToken);
}
