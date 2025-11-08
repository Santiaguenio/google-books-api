using GoogleBooks.Infrastructure.Readers.Mappers.InternalMappings;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Infrastructure.Readers.Mappers;

internal static class ReaderMappersExtensions
{
    internal static void RegisterReaderMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ReadersByCriteriaProfile));
    }
}