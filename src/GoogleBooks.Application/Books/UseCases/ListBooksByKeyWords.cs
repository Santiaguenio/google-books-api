using GoogleBooks.Application.Books.Models;
using GoogleBooks.Application.Common;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts;
using GoogleBooks.Contracts.Requests.Books;
using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Application.Books.UseCases;

internal class ListBooksByKeyWords(
    IBookService bookService,
    IGoogleBooksMapper mapper) : IListByCriteria<BooksSearchCriteria>
{
    public async Task<IGoogleBooksResponse> DoAsync(BooksSearchCriteria request, CancellationToken cancellationToken)
    {
        return mapper.Map<EntitiesByCriteriaDto<BookFullDto>>(await bookService.ListByKeyWordsAsync(
            new ListByKeyWordsParams(
                request.Page,
                request.PageSize,
                request.KeyWords),
            cancellationToken));
    }
}
