using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Contracts.Responses;

namespace GoogleBooks.Infrastructure.Mappers;

internal class BooksByCriteriaProfile : Profile
{
    public BooksByCriteriaProfile()
    {
        CreateMap<Volumes, BooksByKeyWordsDto>();
    }
}
