using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Responses;
using GoogleBooks.Domain.Exceptions;

namespace GoogleBooks.Application.Books.UseCases
{
    internal class GetBookById(IBookService bookService) : IGetById<string>
    {
        public async Task<IGoogleBooksResponse> DoAsync(string id, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new BadRequestException("Provided id can't be null or empty");
            }

            return await bookService.GetByIdAsync(id, cancellationToken);
        }
    }
}
