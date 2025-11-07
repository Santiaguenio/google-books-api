using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Application.Readers.Models;
using GoogleBooks.Contracts.Requests.Readers;
using GoogleBooks.Contracts.Responses.Books;

namespace GoogleBooks.Application.Readers.UseCases;

internal class ListReadersByCriteria(IReaderService readerService) : IListByCriteria<ReadersSearchCriteriaDto>
{
    public async Task<IGoogleBooksResponse> DoAsync(ReadersSearchCriteriaDto request, CancellationToken cancellationToken)
    {

        var a = await readerService.ListByCriteriaAsync(new ListByCriteriaParams(request.Page, request.PageSize), cancellationToken);

        throw new NotImplementedException();
    }
}