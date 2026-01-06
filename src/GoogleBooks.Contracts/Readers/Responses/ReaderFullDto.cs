using GoogleBooks.Contracts.Common;

namespace GoogleBooks.Contracts.Readers.Responses
{
    public record ReaderFullDto : IGoogleBooksResponse
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