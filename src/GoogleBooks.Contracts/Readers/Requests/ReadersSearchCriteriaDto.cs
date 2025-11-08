using GoogleBooks.Contracts.Common;

namespace GoogleBooks.Contracts.Readers.Requests;

public record ReadersSearchCriteriaDto : PagedQueryBase, IGoogleBooksRequest
{
    public string? City { get; set; }
    public string? Name { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? ZipCode { get; set; }

    public DateOnly? BirthDate { get; set; }
}
