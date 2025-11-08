using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Common;
using GoogleBooks.Contracts.Readers.Requests;
using GoogleBooks.Contracts.Readers.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GoogleBooks.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ReadersController : ControllerBase
{
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> CreateAsync(
        [FromServices] ICreate<ReaderCreationDto, int> createReader,
        ReaderCreationDto readerCreation,
        CancellationToken cancellationToken)
    {
        var createdReaderId = await createReader.DoAsync(readerCreation, cancellationToken);
        return Created($"/api/readers/{createdReaderId}", createdReaderId);
    }

    [HttpGet("{id}")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ReaderDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> GetByIdAsync(
        [FromServices] IGetById<int> getById,
        int id,
        CancellationToken cancellationToken)
    {
        return Ok(await getById.DoAsync(id, cancellationToken));
    }

    [HttpGet()]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntitiesByCriteriaDto<ReaderDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ProblemDetails))]
    public async Task<IActionResult> ListByCriteriaAsync(
        [FromServices] IListByCriteria<ReadersSearchCriteriaDto> listByCriteria,
        [FromQuery] ReadersSearchCriteriaDto pageParams,
        CancellationToken cancellationToken)
    {
        return Ok(await listByCriteria.DoAsync(pageParams, cancellationToken));
    }
}
