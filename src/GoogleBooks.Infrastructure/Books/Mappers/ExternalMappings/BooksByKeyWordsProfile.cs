using AutoMapper;
using GoogleBooks.Application.Books.Models;
using GoogleBooks.Contracts.Books.Responses;

namespace GoogleBooks.Infrastructure.Books.Mappers.ExternalMappings;

internal class BooksByKeyWordsProfile : Profile
{
    public BooksByKeyWordsProfile()
    {
        CreateMap<BookFull, BookFullDto>();
    }
}
