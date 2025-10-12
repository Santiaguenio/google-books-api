using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Contracts.Responses;

namespace GoogleBooks.Infrastructure.Books.Mappers;

internal class BookByIdProfile : Profile
{
    public BookByIdProfile()
    {
        CreateMap<Volume, BookFullDto>();
        CreateMap<Volume.VolumeInfoData, VolumeInfo>();
        CreateMap<Volume.VolumeInfoData.ImageLinksData, ImageLinks>();
        CreateMap<Volume.SaleInfoData, SaleInfo>();
    }
}
