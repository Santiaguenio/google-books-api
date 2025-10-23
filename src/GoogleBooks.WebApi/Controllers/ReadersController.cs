using GoogleBooks.Application.Common.UseCases;
using GoogleBooks.Contracts.Requests.Readers;
using GoogleBooks.Contracts.Responses.Readers;
using Microsoft.AspNetCore.Mvc;

namespace GoogleBooks.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ReadersController : ControllerBase
{
    [HttpPost]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
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
}
