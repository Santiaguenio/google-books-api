using AutoMapper;
using GoogleBooks.Application.Common;

namespace GoogleBooks.Infrastructure.Common.Mappers
{
    internal class GoogleBooksMapper(IMapper mapper) : IGoogleBooksMapper
    {
        public TTargetedEntity Map<TTargetedEntity>(object sourceEntity)
        {
            return mapper.Map<TTargetedEntity>(sourceEntity);
        }
    }
}
