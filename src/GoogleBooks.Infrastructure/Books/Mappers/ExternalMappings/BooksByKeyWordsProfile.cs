using AutoMapper;
using GoogleBooks.Application.Books.Models;
using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Infrastructure.Books.Mappers.ExternalMappings;

internal class BooksByKeyWordsProfile : Profile
{
    public BooksByKeyWordsProfile()
    {
        CreateMap<BookFull, BookFullDto>();
    }
}
