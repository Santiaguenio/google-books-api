using FluentValidation;
using GoogleBooks.Infrastructure.Common.Validators;
using GoogleBooks.Infrastructure.Readers.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Unit.Tests;

public class TestFactory
{
    private readonly ServiceProvider _serviceProvider;

    public TestFactory()
    {
        var serviceCollection = new ServiceCollection()
            .AddValidatorsFromAssemblyContaining<ReaderCreationValidator>()
            .AddScoped(typeof(Application.Common.Validators.IGoogleBooksValidator<>), typeof(GoogleBooksValidator<>));

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    internal TService GetRequiredService<TService>() where TService : class
    {
        return _serviceProvider.GetRequiredService<TService>();
    }
}
