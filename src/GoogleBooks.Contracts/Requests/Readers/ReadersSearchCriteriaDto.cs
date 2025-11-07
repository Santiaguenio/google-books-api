namespace GoogleBooks.Contracts.Requests.Readers;

public record ReadersSearchCriteriaDto : PagedQueryBase, IGoogleBooksRequest
{
    public string? City { get; set; }
    public string? Name { get; set; } = default!;
    public string? LastName { get; set; } = default!;

    public string? ZipCode { get; set; }

    public DateOnly? BirthDate { get; set; }
}
