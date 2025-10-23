using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Contracts.Responses.Readers
{
    public record ReaderDto : IGoogleBooksResponse
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
}