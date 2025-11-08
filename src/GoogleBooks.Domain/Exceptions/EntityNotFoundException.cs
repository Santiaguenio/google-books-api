namespace GoogleBooks.Domain.Exceptions
{
    [Serializable]
    public class EntityNotFoundException(string? message) : Exception(message)
    {
    }
}