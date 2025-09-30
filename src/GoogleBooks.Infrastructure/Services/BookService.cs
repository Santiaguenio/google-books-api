using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Application.Books;
using GoogleBooks.Contracts.Responses;
using System.Net.Http.Json;

namespace GoogleBooks.Infrastructure.Services;

internal class BookService(
    IHttpClientFactory httpClientFactory,
    IMapper mapper) : IBookService
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient(ServicesConstants.GoogleClientName);

    public async Task<BookFullDto> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken)
    {
        return mapper.Map<BookFullDto>(await httpClient.GetFromJsonAsync<Volume>($"volumes/{id}", cancellationToken));
    }
}