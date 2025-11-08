using GoogleBooks.Contracts.Common;

namespace GoogleBooks.Contracts.Books.Requests;

public record BooksSearchCriteria : PagedQueryBase, IGoogleBooksRequest
{
    public string KeyWords { get; set; } = default!;
}
