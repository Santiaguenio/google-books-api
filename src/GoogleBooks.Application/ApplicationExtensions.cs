using GoogleBooks.Application.Books;
using GoogleBooks.Application.Common;
using GoogleBooks.Application.Readers;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleBooks.Application
{
    public static class ApplicationExtensions
    {
        public static void RegisterApplicationServices(this IServiceCollection services)
        {
            services.RegisterBookDependencies();
            services.RegisterCommonDependencies();
            services.RegisterReaderDependencies();
        }
    }
}
