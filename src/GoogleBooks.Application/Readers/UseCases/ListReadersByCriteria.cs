using GoogleBooks.Application.Common;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Application.Readers.Models;
using GoogleBooks.Contracts;
using GoogleBooks.Contracts.Requests.Readers;
using GoogleBooks.Contracts.Responses.Books;
using GoogleBooks.Contracts.Responses.Readers;

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