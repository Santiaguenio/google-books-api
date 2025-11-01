namespace GoogleBooks.Application.Common.Validators;

public interface IGoogleBooksValidator<TEntity>
{
    Task<GoogleBooksValidationResult> ValidateAsync(TEntity entity, CancellationToken cancellationToken);
}
