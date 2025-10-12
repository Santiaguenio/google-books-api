using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Requests;
using GoogleBooks.Contracts.Responses;
using GoogleBooks.Domain.Books;
using GoogleBooks.Domain.Exceptions;

namespace GoogleBooks.Application.Books.UseCases;

internal class ListBooksByKeyWords(IBookService bookService) : IListByCriteria<PageParams>
{
    public async Task<IGoogleBooksResponse> DoAsync(PageParams request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.KeyWords))
        {
            throw new BadRequestException("Provided keywords can't be null or empty");
        }

        if (request.PageSize is null || request.PageSize > BookConstants.MaximalItemsPerPage)
        {
            request.PageSize = BookConstants.MaximalItemsPerPage;
        }

        return await bookService.ListByKeyWordsAsync(request, cancellationToken);
    }
}
