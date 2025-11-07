using FluentValidation;
using GoogleBooks.Application.Common.Validators;

namespace GoogleBooks.Infrastructure.Common.Validators;

internal class GoogleBooksValidator<TTEntity>(IValidator<TTEntity> validator) : IGoogleBooksValidator<TTEntity>
{
    public async Task<GoogleBooksValidationResult> ValidateAsync(TTEntity entity, CancellationToken cancellationToken)
    {
        var fluentValidationResult = await validator.ValidateAsync(entity, cancellationToken);

        return new GoogleBooksValidationResult
        {
            Errors = fluentValidationResult.Errors.ToDictionary()
        };
    }
}
