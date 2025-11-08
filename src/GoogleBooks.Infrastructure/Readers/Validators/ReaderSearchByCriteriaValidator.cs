using FluentValidation;
using GoogleBooks.Contracts.Readers.Requests;

namespace GoogleBooks.Infrastructure.Readers.Validators
{
    public class ReaderSearchByCriteriaValidator : AbstractValidator<ReadersSearchCriteriaDto>
    {
        public ReaderSearchByCriteriaValidator()
        {
            RuleFor(_ => _.Email)
                .EmailAddress().WithMessage("Value \"{PropertyValue}\" is not a valid email address");
        }
    }
}
