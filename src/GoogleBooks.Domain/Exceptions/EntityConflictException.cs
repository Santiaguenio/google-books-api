namespace GoogleBooks.Domain.Exceptions
{
    [Serializable]
    public class EntityConflictException(string? message) : Exception(message)
    {
    }
}