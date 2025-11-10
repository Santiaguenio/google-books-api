using GoogleBooks.Application.Common.Models;

namespace GoogleBooks.Application.Readers.Models;

public class ReadersSearchCriteria : PaginationBase
{
    private const int MAXIMAL_PAGE_SIZE = 50;

    public ReadersSearchCriteria(
        int? page,
        int? pageSize,
        string? city,
        string? email,
        string? name,
        string? lastName,
        string? zipCode,
        DateOnly? birthDate)
    {
        Page = page == 0 || page is null ? 1 : page.Value;
        PageSize = pageSize is null || pageSize > MAXIMAL_PAGE_SIZE ? MAXIMAL_PAGE_SIZE : pageSize.Value;

        City = city?.Trim();
        Email = email?.Trim();
        Name = name?.Trim();
        LastName = lastName?.Trim();
        ZipCode = zipCode?.Trim();
        BirthDate = birthDate;
    }

    public string? City { get; private set; }
    public string? Email { get; private set; }
    public string? Name { get; private set; }
    public string? LastName { get; private set; }
    public string? ZipCode { get; private set; }

    public DateOnly? BirthDate { get; private set; }
}
