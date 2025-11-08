using GoogleBooks.Application.Books.Models;
using GoogleBooks.Application.Common.Models;
using GoogleBooks.Application.Common.Services;

namespace GoogleBooks.Application.Books;

public interface IBookService : IService<BookFull>
{
    Task<EntitiesByCriteria<BookFull>> ListByKeyWordsAsync(BooksSearchCriteria request, CancellationToken cancellationToken);
}
