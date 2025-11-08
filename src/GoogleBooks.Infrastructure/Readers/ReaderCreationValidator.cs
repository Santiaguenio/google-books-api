using FluentValidation;
using GoogleBooks.Contracts.Readers.Requests;

namespace GoogleBooks.Infrastructure.Readers;

public class ReaderCreationValidator : AbstractValidator<ReaderCreationDto>
{
    public ReaderCreationValidator()
    {
        RuleFor(_ => _.Email)
            .NotEmpty().WithMessage("Email is mandatory")
            .EmailAddress().WithMessage("Value \"{PropertyValue}\" is not a valid email address");

        RuleFor(_ => _.Name)
            .NotEmpty().WithMessage("Name is mandatory");

        RuleFor(_ => _.LastName)
            .NotEmpty().WithMessage("Last name is mandatory");
    }
}
