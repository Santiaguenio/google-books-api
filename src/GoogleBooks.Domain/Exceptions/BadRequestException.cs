namespace GoogleBooks.Domain.Exceptions
{
    [Serializable]
    public class BadRequestException : Exception
    {
        public IReadOnlyDictionary<string, string[]>? Errors { get; private set; }

        public BadRequestException()
        {
        }

        public BadRequestException(string? message) : base(message)
        {
        }

        public BadRequestException(IReadOnlyDictionary<string, string[]> errors) : base()
        {
            Errors = errors;
        }

        public BadRequestException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}