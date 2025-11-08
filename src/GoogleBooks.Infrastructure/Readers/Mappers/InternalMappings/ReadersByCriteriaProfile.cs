using AutoMapper;
using GoogleBooks.Application.Readers.Models;
using GoogleBooks.Domain.Readers.Entities;

namespace GoogleBooks.Infrastructure.Readers.Mappers.InternalMappings;

internal class ReadersByCriteriaProfile : Profile
{
    public ReadersByCriteriaProfile()
    {
        CreateMap<Reader, ReaderFull>();
    }
}
