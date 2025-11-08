using GoogleBooks.Application.Common.Services;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Application.Common.Validators;
using GoogleBooks.Contracts.Readers.Requests;
using GoogleBooks.Domain.Exceptions;
using GoogleBooks.Domain.Readers.Entities;

namespace GoogleBooks.Application.Readers.UseCases;

internal class CreateReader(
    IGoogleBooksValidator<ReaderCreationDto> validator,
    IDateTimeProvider dateTimeProvider,
    IReaderService readerService) : ICreate<ReaderCreationDto, int>
{
    public async Task<int> DoAsync(ReaderCreationDto readerCreation, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(readerCreation, cancellationToken);
        if (validationResult.IsValid is false)
        {
            throw new BadRequestException(validationResult.Errors);
        }

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