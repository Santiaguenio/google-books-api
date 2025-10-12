using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Contracts;
using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Infrastructure.Books.Mappers;

internal class BooksByKeyWordsProfile : Profile
{
    public BooksByKeyWordsProfile()
    {
        CreateMap<Volumes, EntitiesByCriteriaDto<BookFullDto>>();
    }
}
