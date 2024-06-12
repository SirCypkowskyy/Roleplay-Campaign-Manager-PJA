using Microsoft.AspNetCore.Mvc;

namespace MasFinalProj.API.Controllers;

/// <summary>
/// Testowy kontroler.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    /// <summary>
    /// Testowy endpoint do sprawdzenia działania API.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("Test");
    }
}