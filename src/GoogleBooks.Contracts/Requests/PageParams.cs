namespace GoogleBooks.Contracts.Requests;

public class PageParams : IGoogleBooksRequest
{
    public string KeyWords { get; set; } = default!;

    public int? Page { get; set; } = 0;
    public int? PageSize { get; set; }
}