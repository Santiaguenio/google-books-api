namespace GoogleBooks.Contracts.Responses.Books;

public record BookFullDto : IGoogleBooksResponse
{
    public string Id { get; set; } = default!;
    public string SelfLink { get; set; } = default!;

    public SaleInfoDto SaleInfo { get; set; } = default!;

    public VolumeInfoDto VolumeInfo { get; set; } = default!;
}
