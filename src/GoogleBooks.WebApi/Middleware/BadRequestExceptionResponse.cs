namespace GoogleBooks.WebApi.Middleware;

internal class BadRequestExceptionResponse(int status, IReadOnlyDictionary<string, string[]> errors)
{
    public int Status { get; set; } = status;
    public IReadOnlyDictionary<string, string[]> Errors { get; private set; } = errors;
}