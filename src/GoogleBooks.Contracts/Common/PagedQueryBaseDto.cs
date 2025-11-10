namespace GoogleBooks.Contracts.Common;

public abstract record PagedQueryBaseDto
{
    public int? Page { get; set; } = 1;
    public int? PageSize { get; set; }
}