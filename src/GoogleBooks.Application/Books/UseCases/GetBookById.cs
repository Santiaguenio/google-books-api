using AutoMapper;
using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Responses.Books;
using GoogleBooks.Domain.Exceptions;

namespace GoogleBooks.Application.Books.UseCases;

internal class GetBookById(
    IBookService bookService,
    IMapper mapper) : IGetById<string>
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