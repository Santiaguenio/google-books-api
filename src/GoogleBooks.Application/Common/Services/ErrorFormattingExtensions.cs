using FluentValidation.Results;

namespace GoogleBooks.Application.Common.Services;

public static class ErrorFormattingExtensions
{
    public static IReadOnlyDictionary<string, string[]> ToDictionary(this IEnumerable<ValidationFailure> failures) =>
        failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray());
}