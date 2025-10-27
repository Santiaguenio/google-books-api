namespace GoogleBooks.Application.Books.Models;

public record BookFull
{
    public string Id { get; set; } = default!;
    public string SelfLink { get; set; } = default!;

    public SaleInfo SaleInfo { get; set; } = default!;

    public VolumeInfo VolumeInfo { get; set; } = default!;
}
