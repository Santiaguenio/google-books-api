namespace GoogleBooks.Contracts.Books.Responses
{
    public record VolumeInfoDto
    {
        public string? CanonicalVolumeLink { get; set; }
        public string? Description { get; set; }
        public string? InfoLink { get; set; }
        public string? Language { get; set; }
        public string? PreviewLink { get; set; }
        public string? PublishedDate { get; set; }
        public string? Publisher { get; set; }
        public string Title { get; set; } = default!;

        public int? PageCount { get; set; }
        public int? RatingsCount { get; set; }

        public string[]? Authors { get; set; } = default!;

        public ImageLinksDto ImageLinks { get; set; } = default!;
    }
}