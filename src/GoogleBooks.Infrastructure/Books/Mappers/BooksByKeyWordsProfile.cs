using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Contracts.Responses;

namespace GoogleBooks.Infrastructure.Books.Mappers;

internal class BooksByKeyWordsProfile : Profile
{
    public BooksByKeyWordsProfile()
    {
        CreateMap<Volumes, BooksByKeyWordsDto>();
    }
}
