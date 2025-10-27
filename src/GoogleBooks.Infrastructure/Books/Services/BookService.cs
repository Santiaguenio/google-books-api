using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Application.Books;
using GoogleBooks.Application.Books.Models;
using GoogleBooks.Contracts;
using GoogleBooks.Contracts.Responses.Books;
using System.Net.Http.Json;

namespace GoogleBooks.Infrastructure.Books.Services;

internal class BookService(
    IHttpClientFactory httpClientFactory,
    IMapper mapper) : IBookService
{
    private readonly HttpClient httpClient = httpClientFactory.CreateClient(ServicesConstants.GOOGLE_CLIENT_NAME);

    public async Task<BookFullDto> GetByIdAsync<TKey>(TKey id, CancellationToken cancellationToken)
    {
        return mapper.Map<BookFullDto>(await httpClient.GetFromJsonAsync<Volume>($"volumes/{id}", cancellationToken));
    }

    public async Task<EntitiesByCriteriaDto<BookFullDto>> ListByKeyWordsAsync(ListByKeyWordsParams request, CancellationToken cancellationToken)
    {
        return mapper.Map<EntitiesByCriteriaDto<BookFullDto>>(await httpClient.GetFromJsonAsync<Volumes>($"volumes?" +
                 $"q={request.KeyWords}" +
                 $"&maxResults={request.PageSize}" +
                 $"&startIndex={(request.Page - 1) * request.PageSize}",
             cancellationToken));
    }
}