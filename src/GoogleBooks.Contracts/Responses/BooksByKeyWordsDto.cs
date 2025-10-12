namespace GoogleBooks.Contracts.Responses;

public class BooksByKeyWordsDto : IGoogleBooksResponse
{
    public int TotalItems { get; set; }

    public BookFullDto[] Items { get; set; } = default!;
}
