using GoogleBooks.Application.Common.Models;
using GoogleBooks.Contracts.Books.Requests;
using GoogleBooks.Domain.Books;
using GoogleBooks.Domain.Exceptions;

namespace GoogleBooks.Application.Books.Models;

public class ListByKeyWordsParams : PaginationBase
{
    public string KeyWords { get; private set; }

    public ListByKeyWordsParams(
        int? page,
        int? pageSize,
        string keyWords)
    {
        if (string.IsNullOrWhiteSpace(keyWords))
        {
            throw new BadRequestException(new Dictionary<string, string[]> { { nameof(BooksSearchCriteria.KeyWords), [$"{nameof(BooksSearchCriteria.KeyWords)} is mandatory"] } });
        }

        if (pageSize is null || pageSize > BookConstants.MAXIMAL_ITEMS_PER_PAGE)
        {
            pageSize = BookConstants.MAXIMAL_ITEMS_PER_PAGE;
        }

        KeyWords = keyWords;
        Page = page is null ? 0 : page.Value;
        PageSize = pageSize.Value;
    }
}
