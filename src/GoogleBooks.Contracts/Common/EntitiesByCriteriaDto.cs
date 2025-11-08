using GoogleBooks.Contracts.Books.Responses;

namespace GoogleBooks.Contracts.Common;

public class EntitiesByCriteriaDto<TEntities> : IGoogleBooksResponse
{
    public int TotalItems { get; set; }

    public TEntities[] Items { get; set; } = default!;
}
