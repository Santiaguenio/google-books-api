namespace GoogleBooks.Contracts.Common;

public record EntitiesByCriteriaDto<TEntities> : IGoogleBooksResponse
{
    public int TotalItems { get; set; }

    public TEntities[] Items { get; set; } = default!;
}
