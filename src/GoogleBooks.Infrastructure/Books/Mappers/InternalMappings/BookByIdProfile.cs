using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Application.Books.Models;

namespace GoogleBooks.Infrastructure.Books.Mappers.InternalMappings;

internal class BookByIdProfile : Profile
{
    public BookByIdProfile()
    {
        CreateMap<Volume, BookFull>();
        CreateMap<Volume.VolumeInfoData, VolumeInfo>();
        CreateMap<Volume.VolumeInfoData.ImageLinksData, ImageLinks>();
        CreateMap<Volume.SaleInfoData, SaleInfo>();
    }
}
