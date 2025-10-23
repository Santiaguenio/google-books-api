using FluentValidation;
using GoogleBooks.Application.Readers.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Unit.Tests;

public class TestFactory
{
    private readonly ServiceProvider _serviceProvider;

    public TestFactory()
    {
        var serviceCollection = new ServiceCollection()
            .AddValidatorsFromAssemblyContaining<ReaderCreationValidator>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    internal T GetRequiredService<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<T>();
    }
}
