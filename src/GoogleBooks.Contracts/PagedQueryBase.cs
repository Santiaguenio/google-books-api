namespace GoogleBooks.Contracts;

public abstract record PagedQueryBase
{
    public int? Page { get; set; } = 0;
    public int? PageSize { get; set; }
}