using GoogleBooks.Application.Common.Services;
using GoogleBooks.Contracts;
using GoogleBooks.Contracts.Requests.Books;
using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Application.Books;

public interface IBookService : IService<BookFullDto>
{
    Task<EntitiesByCriteriaDto<BookFullDto>> ListByKeyWordsAsync(PageParams request, CancellationToken cancellationToken);
}
