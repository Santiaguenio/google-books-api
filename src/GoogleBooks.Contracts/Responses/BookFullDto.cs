namespace GoogleBooks.Contracts.Responses;

public record BookFullDto : IGoogleBooksResponse
{
    public string Etag { get; set; } = default!;
    public string Id { get; set; } = default!;
    public string SelfLink { get; set; } = default!;

    public SaleInfo SaleInfo { get; set; } = default!;

    public VolumeInfo VolumeInfo { get; set; } = default!;
}
