namespace GoogleBooks.Application.Common.Models;

public class EntitiesByCriteria<TEntities>
{
    public int TotalItems { get; set; }

    public TEntities[] Items { get; set; } = default!;
}
