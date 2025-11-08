using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Application.Books;
using GoogleBooks.Application.Books.Models;
using GoogleBooks.Application.Common.Models;
using System.Net.Http.Json;

namespace GoogleBooks.Infrastructure.Books.Services;

internal class BookService(
    IHttpClientFactory httpClientFactory,
    IMapper mapper) : IBookService
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient(ServicesConstants.GOOGLE_CLIENT_NAME);

    public async Task<BookFull> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken)
    {
        return mapper.Map<BookFull>(await httpClient.GetFromJsonAsync<Volume>($"volumes/{id}", cancellationToken));
    }

    public async Task<EntitiesByCriteria<BookFull>> ListByKeyWordsAsync(BooksSearchCriteria request, CancellationToken cancellationToken)
    {
        return mapper.Map<EntitiesByCriteria<BookFull>>(await httpClient.GetFromJsonAsync<Volumes>($"volumes?" +
                 $"q={request.KeyWords}" +
                 $"&maxResults={request.PageSize}" +
                 $"&startIndex={(request.Page - 1) * request.PageSize}",
             cancellationToken));
    }
}