using FluentValidation;
using GoogleBooks.Application.Common.Validators;
using GoogleBooks.Infrastructure.Readers.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Common.Validators;

internal static class ValidatorExtensions
{
    internal static void RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(ReaderCreationValidator));

        services.AddScoped(typeof(IGoogleBooksValidator<>), typeof(GoogleBooksValidator<>));
    }
}
