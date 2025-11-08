using GoogleBooks.Application.Books.Models;
using GoogleBooks.Application.Common;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Books.Requests;
using GoogleBooks.Contracts.Books.Responses;
using GoogleBooks.Contracts.Common;

namespace GoogleBooks.Application.Books.UseCases;

internal class ListBooksByKeyWords(
    IBookService bookService,
    IGoogleBooksMapper mapper) : IListByCriteria<BooksSearchCriteriaDto>
{
    public async Task<IGoogleBooksResponse> DoAsync(BooksSearchCriteriaDto request, CancellationToken cancellationToken)
    {
        return mapper.Map<EntitiesByCriteriaDto<BookFullDto>>(await bookService.ListByKeyWordsAsync(
            new BooksSearchCriteria(
                request.Page,
                request.PageSize,
                request.KeyWords),
            cancellationToken));
    }
}
