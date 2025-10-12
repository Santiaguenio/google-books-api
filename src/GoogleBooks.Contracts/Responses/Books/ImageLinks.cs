namespace GoogleBooks.Contracts.Responses.Books
{
    public record ImageLinks
    {
        public string? ExtraLarge { get; set; }
        public string? Medium { get; set; }
        public string? Large { get; set; }
        public string? Small { get; set; }
        public string? SmallThumbnail { get; set; }
        public string? Thumbnail { get; set; }
    }
}