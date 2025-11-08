using GoogleBooks.Contracts.Common;
using System.ComponentModel.DataAnnotations;

namespace GoogleBooks.Contracts.Readers.Requests;

public record ReaderCreationDto : IGoogleBooksRequest
{
    public string? Address { get; set; }
    public string? City { get; set; }

    [EmailAddress]
    public string Email { get; set; } = default!;
   
    public string Name { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? ZipCode { get; set; }

    public DateOnly Birthdate { get; set; }
}