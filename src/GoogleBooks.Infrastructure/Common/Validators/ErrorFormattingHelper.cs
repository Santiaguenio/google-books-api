using FluentValidation.Results;
using GoogleBooks.Infrastructure.Common.Validators;

namespace GoogleBooks.Infrastructure.Common.Validators;

public static class ErrorFormattingHelper
{
    public static Dictionary<string, string[]> ToDictionary(this IEnumerable<ValidationFailure> failures) =>
        failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray());
}