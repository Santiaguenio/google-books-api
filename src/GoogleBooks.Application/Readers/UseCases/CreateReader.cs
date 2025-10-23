using FluentValidation;
using GoogleBooks.Application.Common.Services;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Requests.Readers;
using GoogleBooks.Domain.Exceptions;
using GoogleBooks.Domain.Readers.Entities;

namespace GoogleBooks.Application.Readers.UseCases;

internal class CreateReader(
    IValidator<ReaderCreationDto> validator,
    IDateTimeProvider dateTimeProvider,
    IReaderService readerService) : ICreate<ReaderCreationDto, int>
{
    public async Task<int> DoAsync(ReaderCreationDto readerCreation, CancellationToken cancellationToken)
    {
        var validationResult = await validator.ValidateAsync(readerCreation, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult.Errors.ToDictionary());
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