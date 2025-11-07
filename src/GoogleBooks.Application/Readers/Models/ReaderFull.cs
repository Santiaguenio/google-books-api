namespace GoogleBooks.Application.Readers.Models;

public class ReaderFull
{
    public int Id { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string Email { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string? ZipCode { get; set; }

    public DateOnly Birthdate { get; set; }
}
