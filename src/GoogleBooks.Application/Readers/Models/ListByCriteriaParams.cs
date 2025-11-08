using GoogleBooks.Application.Common.Models;

namespace GoogleBooks.Application.Readers.Models;

public class ListByCriteriaParams : PaginationBase
{
    public ListByCriteriaParams(
        int? page,
        int? pageSize,
        string? city,
        string? name,
        string? lastName,
        string? zipCode,
        DateOnly? birthDate)
    {
        Page = page is null ? 0 : page.Value;
        PageSize = pageSize is null ? 50 : pageSize.Value;

        City = city;
        Name = name;
        LastName = lastName;
        ZipCode = zipCode;
        BirthDate = birthDate;
    }

    public string? City { get; set; }
    public string? Name { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? ZipCode { get; set; }

    public DateOnly? BirthDate { get; set; }
}
