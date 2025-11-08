using AutoMapper;
using GoogleBooks.Application.Books.Models;
using GoogleBooks.Contracts.Books.Responses;

namespace GoogleBooks.Infrastructure.Books.Mappers.ExternalMappings;

internal class BookByIdProfile : Profile
{
    public BookByIdProfile()
    {
        CreateMap<BookFull, BookFullDto>();
        CreateMap<VolumeInfo, VolumeInfoDto>();
        CreateMap<ImageLinks, ImageLinksDto>();
        CreateMap<SaleInfo, SaleInfoDto>();
    }
}
