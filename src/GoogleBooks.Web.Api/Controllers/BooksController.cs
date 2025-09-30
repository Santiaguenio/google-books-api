using GoogleBooks.Application.Common.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace GoogleBooks.WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(
        [FromServices] IGetById<string> getById,
        string id,
        CancellationToken cancellationToken)
    {
        return Ok(await getById.DoAsync(id, cancellationToken));
    }
}
