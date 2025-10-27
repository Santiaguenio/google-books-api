using GoogleBooks.Application.Books.Models;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Requests.Books;
using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Application.Books.UseCases;

internal class ListBooksByKeyWords(IBookService bookService) : IListByCriteria<PageParamsDto>
{
    public async Task<IGoogleBooksResponse> DoAsync(PageParamsDto request, CancellationToken cancellationToken)
    {
        return await bookService.ListByKeyWordsAsync(
            new ListByKeyWordsParams(
                request.Page,
                request.PageSize,
                request.KeyWords),
            cancellationToken);
    }
}
