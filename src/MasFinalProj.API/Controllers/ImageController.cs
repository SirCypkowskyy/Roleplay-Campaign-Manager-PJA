using MasFinalProj.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MasFinalProj.API.Controllers;

/// <summary>
/// Kontroler do zarządzania obrazami
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly ILogger<ImageController> _logger;
    private readonly IImageRepository _imageRepository;
    
    public ImageController(ILogger<ImageController> logger, IImageRepository imageRepository)
    {
        _logger = logger;
        _imageRepository = imageRepository;
    }
    
    /// <summary>
    /// Pobiera obraz o podanym id
    /// </summary>
    /// <param name="id">
    /// Id obrazu
    /// </param>
    /// <returns>
    /// Obraz w formacie base64
    /// </returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage([FromRoute] long id)
    {
        var image = await _imageRepository.GetByIdAsync(id);
        if (image is null)
            return NotFound();
        return File(Convert.FromBase64String(image.Base64Image), $"image/{image.ImageFormat}");
    }
}