using FluentValidation;
using GoogleBooks.Infrastructure.Readers;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Common.Validators;

internal static class ValidatorExtensions
{
    internal static void RegisterValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(ReaderCreationValidator));

        services.AddScoped(typeof(Application.Common.Validators.IGoogleBooksValidator<>), typeof(GoogleBooksValidator<>));
    }
}
