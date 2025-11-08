using GoogleBooks.Application.Common;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Books.Responses;
using GoogleBooks.Contracts.Common;
using GoogleBooks.Domain.Exceptions;

namespace GoogleBooks.Application.Books.UseCases;

internal class GetBookById(
    IBookService bookService,
    IGoogleBooksMapper mapper) : IGetById<string>
{
    public async Task<IGoogleBooksResponse> DoAsync(string id, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new BadRequestException(new Dictionary<string, string[]> { { "Id", ["Id is mandatory"] } });
        }

        return mapper.Map<BookFullDto>(await bookService.GetByIdAsync(id, cancellationToken));
    }
}