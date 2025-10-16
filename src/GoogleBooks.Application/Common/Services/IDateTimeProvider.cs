namespace GoogleBooks.Application.Common.Services;

internal interface IDateTimeProvider
{
    DateTime UtcNow();
}
