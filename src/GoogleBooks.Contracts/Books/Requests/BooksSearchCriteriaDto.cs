using GoogleBooks.Contracts.Common;

namespace GoogleBooks.Contracts.Books.Requests;

public record BooksSearchCriteriaDto : PagedQueryBaseDto, IGoogleBooksRequest
{
    public string KeyWords { get; set; } = default!;
}
