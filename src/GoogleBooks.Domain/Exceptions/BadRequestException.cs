namespace GoogleBooks.Domain.Exceptions
{
    [Serializable]
    public class BadRequestException(IReadOnlyDictionary<string, string[]> errors) : Exception()
    {
        public IReadOnlyDictionary<string, string[]>? Errors { get; private set; } = errors;
    }
}