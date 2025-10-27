using AutoMapper;
using GoogleBooks.Application.Common.Models;
using GoogleBooks.Contracts;

namespace GoogleBooks.Application.Common.Mappers;

internal class EntitiesByCriteriaProfile : Profile
{
    public EntitiesByCriteriaProfile()
    {
        CreateMap(typeof(EntitiesByCriteria<>), typeof(EntitiesByCriteriaDto<>));
    }
}
