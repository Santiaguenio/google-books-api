namespace GoogleBooks.Application.Books.Models;

public record SaleInfo
{
    public string? BuyLink { get; set; }
    public string? Country { get; set; }
    public string? Saleability { get; set; }
    
    public bool IsEbook { get; set; }
}