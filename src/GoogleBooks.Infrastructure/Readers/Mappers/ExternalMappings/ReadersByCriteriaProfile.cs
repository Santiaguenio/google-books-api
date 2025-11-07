using AutoMapper;
using GoogleBooks.Application.Readers.Models;
using GoogleBooks.Contracts.Responses.Readers;

namespace GoogleBooks.Infrastructure.Readers.Mappers.ExternalMappings;

internal class ReadersByCriteriaProfile : Profile
{
    public ReadersByCriteriaProfile()
    {
        CreateMap<ReaderFull, ReaderDto>();
    }
}
