using GoogleBooks.Contracts.Common;

namespace GoogleBooks.Contracts.Books.Responses;

public record BookFullDto : IGoogleBooksResponse
{
    public string Id { get; set; } = default!;
    public string SelfLink { get; set; } = default!;

    public SaleInfoDto SaleInfo { get; set; } = default!;

    public VolumeInfoDto VolumeInfo { get; set; } = default!;
}
