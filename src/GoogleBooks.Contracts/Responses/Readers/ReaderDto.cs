using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Contracts.Responses.Readers
{
    public record ReaderDto : IGoogleBooksResponse
    {
        public string Address { get; set; } = default!;
        public string? City { get; set; }
        public string Email { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string? ZipCode { get; set; }

        public DateTime Birthdate { get; set; }
    }
}