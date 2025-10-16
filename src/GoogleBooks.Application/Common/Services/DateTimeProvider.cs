namespace GoogleBooks.Application.Common.Services;

internal class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow()
    {
        return DateTime.UtcNow;
    }
}
