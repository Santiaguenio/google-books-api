using AutoMapper;
using GoogleBooks.Application.Books.Models;
using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Infrastructure.Books.Mappers.OutgoingMappings;

internal class BooksByKeyWordsProfile : Profile
{
    public BooksByKeyWordsProfile()
    {
        CreateMap<BookFull, BookFullDto>();
    }
}
