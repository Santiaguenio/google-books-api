using GoogleBooks.Contracts.Common;
using System.ComponentModel.DataAnnotations;

namespace GoogleBooks.Contracts.Readers.Requests;

public record ReadersSearchCriteriaDto : PagedQueryBaseDto, IGoogleBooksRequest
{
    public string? City { get; set; }

    [EmailAddress]
    public string? Email { get; set; }
    
    public string? Name { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? ZipCode { get; set; }

    public DateOnly? BirthDate { get; set; }
}
