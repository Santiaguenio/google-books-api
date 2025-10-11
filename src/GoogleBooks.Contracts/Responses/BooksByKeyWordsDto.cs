namespace GoogleBooks.Contracts.Responses;

public class BooksByKeyWordsDto
{
    public int TotalItems { get; set; }

    public BookFullDto[] Items { get; set; } = default!;
}
