using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Application.Books.Models;
using GoogleBooks.Application.Common.Models;

namespace GoogleBooks.Infrastructure.Books.Mappers.InternalMappings;

internal class BooksByKeyWordsProfile : Profile
{
    public BooksByKeyWordsProfile()
    {
        CreateMap<Volumes, EntitiesByCriteria<BookFull>>();
    }
}
