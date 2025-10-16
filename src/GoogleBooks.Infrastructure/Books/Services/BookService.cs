using AutoMapper;
using Google.Apis.Books.v1.Data;
using GoogleBooks.Application.Books;
using GoogleBooks.Contracts;
using GoogleBooks.Contracts.Requests.Books;
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

    public async Task<EntitiesByCriteriaDto<BookFullDto>> ListByKeyWordsAsync(PageParams pageParams, CancellationToken cancellationToken)
    {
        return mapper.Map<EntitiesByCriteriaDto<BookFullDto>>(await httpClient.GetFromJsonAsync<Volumes>($"volumes?" +
                 $"q={pageParams.KeyWords}" +
                 $"&maxResults={pageParams.PageSize}" +
                 $"&startIndex={(pageParams.Page - 1) * pageParams.PageSize}",
             cancellationToken));
    }
}