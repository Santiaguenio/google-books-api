namespace GoogleBooks.Contracts.Requests.Books;

public record BooksSearchCriteria : PagedQueryBase, IGoogleBooksRequest
{
    public string KeyWords { get; set; } = default!;
}
