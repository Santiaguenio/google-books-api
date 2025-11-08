using GoogleBooks.Application.Common.Models;

namespace GoogleBooks.Application.Readers.Models;

public class ListByCriteriaParams : PaginationBase
{
    private const int MAXIMAL_PAGE_SIZE = 50;

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
        PageSize = pageSize is null || pageSize > MAXIMAL_PAGE_SIZE ? MAXIMAL_PAGE_SIZE : pageSize.Value;

        City = city?.Trim();
        Name = name?.Trim();
        LastName = lastName?.Trim();
        ZipCode = zipCode?.Trim();
        BirthDate = birthDate;
    }

    public string? City { get; set; }
    public string? Name { get; set; } = default!;
    public string? LastName { get; set; } = default!;
    public string? ZipCode { get; set; }

    public DateOnly? BirthDate { get; set; }
}
