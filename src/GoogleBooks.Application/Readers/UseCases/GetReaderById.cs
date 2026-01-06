using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Common;
using GoogleBooks.Contracts.Readers.Responses;
using GoogleBooks.Domain.Exceptions;

namespace GoogleBooks.Application.Readers.UseCases;

internal class GetReaderById(IReaderService readerService) : IGetById<int>
{
    public async Task<IGoogleBooksResponse> DoAsync(int id, CancellationToken cancellationToken)
    {
        var reader = await readerService.GetByIdAsync(id, cancellationToken);
        if (reader is null)
        {
            throw new EntityNotFoundException($"The Reader with Id: {id} was not found");
        }

        return new ReaderFullDto
        {
            Address = reader.Address,
            Birthdate = reader.Birthdate,
            City = reader.City,
            Email = reader.Email,
            Id = reader.Id,
            LastName = reader.LastName,
            Name = reader.Name,
            ZipCode = reader.ZipCode
        };
    }
}
