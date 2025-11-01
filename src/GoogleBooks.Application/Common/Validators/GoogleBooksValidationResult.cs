namespace GoogleBooks.Application.Common.Validators;

public class GoogleBooksValidationResult
{
    public bool IsValid => Errors.Count == 0;

    public Dictionary<string, string[]> Errors { get; set; } = [];
}
