namespace GoogleBooks.Application.Common.Validators
{
    public class GoogleBooksValidationFailure
    {
        public string ErrorMessage { get; set; } = default!;
        public string PropertyName { get; set; } = default!;
    }
}