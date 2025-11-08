using GoogleBooks.Application.Common;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Application.Readers.Models;
using GoogleBooks.Contracts.Books.Responses;
using GoogleBooks.Contracts.Common;
using GoogleBooks.Contracts.Readers.Requests;
using GoogleBooks.Contracts.Readers.Responses;

namespace GoogleBooks.Application.Readers.UseCases;

internal class ListReadersByCriteria(
    IReaderService readerService,
    IGoogleBooksMapper googleBooksMapper) : IListByCriteria<ReadersSearchCriteriaDto>
{
    public async Task<IGoogleBooksResponse> DoAsync(ReadersSearchCriteriaDto request, CancellationToken cancellationToken)
    {
        var readers = await readerService.ListByCriteriaAsync(new ListByCriteriaParams(
                request.Page,
                request.PageSize,
                request.City,
                request.Name,
                request.LastName,
                request.ZipCode,
                request.BirthDate),
            cancellationToken);

        return googleBooksMapper.Map<EntitiesByCriteriaDto<ReaderDto>>(readers);
    }
}