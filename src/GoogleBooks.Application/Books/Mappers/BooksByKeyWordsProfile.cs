using AutoMapper;
using GoogleBooks.Application.Books.Models;
using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Application.Books.Mappers;

internal class BooksByKeyWordsProfile : Profile
{
    public BooksByKeyWordsProfile()
    {
        CreateMap<BookFull, BookFullDto>();
    }
}
