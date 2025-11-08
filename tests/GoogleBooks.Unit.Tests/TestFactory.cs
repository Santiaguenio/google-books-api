using FluentValidation;
using GoogleBooks.Application.Common;
using GoogleBooks.Application.Common.Validators;
using GoogleBooks.Infrastructure.Common.Mappers;
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
            .AddScoped(typeof(IGoogleBooksValidator<>), typeof(GoogleBooksValidator<>))

            .AddAutoMapper(typeof(EntitiesByCriteriaProfile))
            .AddScoped<IGoogleBooksMapper, GoogleBooksMapper>();

        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    internal TService GetRequiredService<TService>() where TService : class
    {
        return _serviceProvider.GetRequiredService<TService>();
    }
}
