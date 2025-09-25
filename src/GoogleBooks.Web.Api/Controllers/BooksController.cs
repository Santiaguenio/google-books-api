using Microsoft.AspNetCore.Mvc;

namespace GoogleBooks.Web.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class BooksController : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync(string id)
    {
        return Ok(id);
    }
}
