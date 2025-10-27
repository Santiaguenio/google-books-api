using GoogleBooks.Contracts.Requests.Books;
using GoogleBooks.Domain.Books;
using GoogleBooks.Domain.Exceptions;

namespace GoogleBooks.Application.Books.Models;

public class ListByKeyWordsParams
{
    public string KeyWords { get; private set; }

    public int? Page { get; private set; }
    public int? PageSize { get; private set; }

    public ListByKeyWordsParams(
        int? page,
        int? pageSize,
        string keyWords)
    {
        if (string.IsNullOrWhiteSpace(keyWords))
        {
            throw new BadRequestException(new Dictionary<string, string[]> { { nameof(PageParamsDto.KeyWords), [$"{nameof(PageParamsDto.KeyWords)} is mandatory"] } });
        }

        if (pageSize is null || pageSize > BookConstants.MaximalItemsPerPage)
        {
            pageSize = BookConstants.MaximalItemsPerPage;
        }

        KeyWords = keyWords;
        Page = page;
        PageSize = pageSize;
    }
}
