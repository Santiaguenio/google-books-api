using GoogleBooks.Application.Common.Services;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Requests.Readers;
using GoogleBooks.Domain.Readers.Entities;

namespace GoogleBooks.Application.Readers.UseCases;

internal class CreateReader(
    IDateTimeProvider dateTimeProvider,
    IReaderService readerService) : ICreate<ReaderCreationDto, int>
{
    public async Task<int> DoAsync(ReaderCreationDto readerCreation, CancellationToken cancellationToken)
    {
        // TODO: Validate entry data

        var now = dateTimeProvider.UtcNow();
        var createdReader = await readerService.AddAsync(new Reader
        {
            Address = readerCreation.Address,
            Birthdate = readerCreation.Birthdate,
            City = readerCreation.City,
            CreationDate = now,
            Email = readerCreation.Email,
            LastName = readerCreation.LastName,
            LastUpdate = now,
            Name = readerCreation.Name,
            ZipCode = readerCreation.ZipCode
        }, cancellationToken);

        return createdReader.Id;
    }
}