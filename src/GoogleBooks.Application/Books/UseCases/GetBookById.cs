using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Responses;

namespace GoogleBooks.Application.Books.UseCases
{
    internal class GetBookById(IBookService bookService) : IGetById<string>
    {
        public async Task<IGoogleBooksResponse> DoAsync(string id, CancellationToken cancellationToken)
        {
            return await bookService.GetByIdAsync(id, cancellationToken);
        }
    }
}
