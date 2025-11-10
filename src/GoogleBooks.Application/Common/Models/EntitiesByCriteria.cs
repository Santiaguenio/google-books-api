namespace GoogleBooks.Application.Common.Models;

public class EntitiesByCriteria<TEntities>
{
    public long TotalItems { get; set; }

    public TEntities[] Items { get; set; } = default!;
}
