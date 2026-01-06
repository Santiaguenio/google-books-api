using GoogleBooks.Application.Common;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Application.Common.Validators;
using GoogleBooks.Application.Readers.Models;
using GoogleBooks.Contracts.Common;
using GoogleBooks.Contracts.Readers.Requests;
using GoogleBooks.Contracts.Readers.Responses;
using GoogleBooks.Domain.Exceptions;

namespace GoogleBooks.Application.Readers.UseCases;

internal class ListReadersByCriteria(
    IGoogleBooksValidator<ReadersSearchCriteriaDto> googleBooksValidator,
    IReaderService readerService,
    IGoogleBooksMapper googleBooksMapper) : IListByCriteria<ReadersSearchCriteriaDto>
{
    public async Task<IGoogleBooksResponse> DoAsync(ReadersSearchCriteriaDto request, CancellationToken cancellationToken)
    {
        var validationResult = await googleBooksValidator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid is false)
        {
            throw new BadRequestException(validationResult.Errors);
        }

        var readers = await readerService.ListByCriteriaAsync(new ReadersSearchCriteria(
                request.Page,
                request.PageSize,
                request.City,
                request.Email,
                request.Name,
                request.LastName,
                request.ZipCode,
                request.BirthDate),
            cancellationToken);

        return googleBooksMapper.Map<EntitiesByCriteriaDto<ReaderFullDto>>(readers);
    }
}