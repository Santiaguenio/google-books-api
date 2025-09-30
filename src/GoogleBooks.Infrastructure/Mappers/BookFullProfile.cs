using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Contracts.Responses;

namespace GoogleBooks.Infrastructure.Mappers;

internal class BookFullProfile : Profile
{
    public BookFullProfile()
    {
        CreateMap<Volume, BookFullDto>();
        CreateMap<Volume.VolumeInfoData, VolumeInfo>();
        CreateMap<Volume.VolumeInfoData.ImageLinksData, ImageLinks>();
        CreateMap<Volume.SaleInfoData, SaleInfo>();
    }
}
