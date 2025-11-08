using AutoMapper;
using GoogleBooks.Application.Common.Models;
using GoogleBooks.Contracts.Common;

namespace GoogleBooks.Infrastructure.Common.Mappers;

internal class EntitiesByCriteriaProfile : Profile
{
    public EntitiesByCriteriaProfile()
    {
        CreateMap(typeof(EntitiesByCriteria<>), typeof(EntitiesByCriteriaDto<>));
    }
}
