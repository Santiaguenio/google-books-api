using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Contracts;

public class EntitiesByCriteriaDto<TEntities> : IGoogleBooksResponse
{
    public int TotalItems { get; set; }

    public TEntities[] Items { get; set; } = default!;
}
