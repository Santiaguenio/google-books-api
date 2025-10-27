namespace GoogleBooks.Contracts.Responses.Books
{
    public record SaleInfoDto
    {
        public string? BuyLink { get; set; }
        public string? Country { get; set; }
        public string? Saleability { get; set; }
        
        public bool IsEbook { get; set; }
    }
}