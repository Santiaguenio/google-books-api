using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Books.Requests;
using GoogleBooks.Contracts.Books.Responses;
using GoogleBooks.Contracts.Common;
using Microsoft.AspNetCore.Mvc;

namespace GoogleBooks.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookFullDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetByIdAsync(
        [FromServices] IGetById<string> getById,
        string id,
        CancellationToken cancellationToken)
    {
        return Ok(await getById.DoAsync(id, cancellationToken));
    }

    [HttpGet()]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntitiesByCriteriaDto<BookFullDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ListByKeyWordsAsync(
        [FromServices] IListByCriteria<BooksSearchCriteria> listByCriteria,
        [FromQuery] BooksSearchCriteria pageParams,
        CancellationToken cancellationToken)
    {
        return Ok(await listByCriteria.DoAsync(pageParams, cancellationToken));
    }
}
