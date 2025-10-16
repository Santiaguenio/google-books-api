using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Responses.Books;
using GoogleBooks.Contracts.Responses.Readers;

namespace GoogleBooks.Application.Readers.UseCases
{
    internal class GetReaderById(IReaderService readerService) : IGetById<int>
    {
        public async Task<IGoogleBooksResponse> DoAsync(int id, CancellationToken cancellationToken)
        {
            var reader = await readerService.GetByIdAsync(id, cancellationToken);

            return new ReaderDto
            {
                Address = reader.Address,
                Birthdate = reader.Birthdate,
                City = reader.City,
                Email = reader.Email,
                LastName = reader.LastName,
                Name = reader.Name,
                ZipCode = reader.ZipCode
            };
        }
    }
}
