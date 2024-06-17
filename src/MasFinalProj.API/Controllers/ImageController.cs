using MasFinalProj.API.Abstractions;
using MasFinalProj.Domain.DTOs;
using MasFinalProj.Domain.DTOs.Image.Output;
using MasFinalProj.Domain.Helpers;
using MasFinalProj.Domain.Models.Common;
using MasFinalProj.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasFinalProj.API.Controllers;

/// <summary>
/// Kontroler do zarządzania obrazami
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class ImageController : AbstractController<long, Image, AbstractOutputDTOs.ImageDto, ImageResponseDTO>
{
    private readonly ILogger<ImageController> _logger;
    private readonly IImageRepository _imageRepository;
    
    public ImageController(ILogger<ImageController> logger, IImageRepository imageRepository) : base(imageRepository)
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
    [ProducesResponseType(typeof(FileContentResult), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetImage([FromRoute] long id)
    {
        try
        {
            var image = await _imageRepository.GetByIdAsync(id);
            if (image is null)
                return NotFound();
            return File(Convert.FromBase64String(image.Base64Image), $"image/{image.ImageFormat}");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting image with id {Id}", id);
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Pobiera wszystkie obrazy
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(List<ImageResponseDTO>), 200)]
    [ProducesResponseType(500)]
    [Authorize]
    public override async Task<ActionResult<List<ImageResponseDTO>>> GetAllAsync()
    {
        try
        {
            var images = await _imageRepository.GetAllAsync();
            return Ok(images);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all images");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Pobiera obraz o podanym id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("abstract/{id}")]
    [ProducesResponseType(typeof(ImageResponseDTO), 200)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public override async Task<ActionResult<ImageResponseDTO>> GetByIdAsync([FromRoute] long id)
    {
        try
        {
            var image = await _imageRepository.GetByIdAsync(id);
            if (image is null)
                return NotFound();
            return Ok(image);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while getting image with id {Id}", id);
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Tworzy nowy obraz
    /// </summary>
    /// <param name="entity">
    /// Obraz do utworzenia
    /// </param>
    /// <returns>
    /// Utworzony obraz
    /// </returns>
    [HttpPost]
    [ProducesResponseType(typeof(ImageResponseDTO), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Authorize]
    public override async Task<ActionResult<ImageResponseDTO>> CreateAsync([FromBody] AbstractOutputDTOs.ImageDto entity)
    {
        try
        {
            var image = await _imageRepository.AddAsync(new Image
            {
                Base64Image = entity.Base64Image,
                Checksum = "",
                ImageFormat = entity.ImageFormat,
                ImageName = entity.ImageName
            });
            return CreatedAtAction(nameof(GetImage), new { id = image.Id }, image);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating image");
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Aktualizuje obraz
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(ImageResponseDTO), 200)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<ImageResponseDTO>> UpdateAsync([FromBody] Image entity)
    {
        try
        {
            var image = await _imageRepository.UpdateAsync(entity);
            return Ok(image);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while updating image with id {Id}", entity.Id);
            return StatusCode(500);
        }
    }

    /// <summary>
    /// Usuwa obraz o podanym id
    /// </summary>
    /// <param name="id">
    /// Id obrazu
    /// </param>
    /// <returns>
    /// Ilość usuniętych obrazów
    /// </returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(int), 200)]
    [ProducesResponseType(500)]
    [Authorize(Roles = "Admin")]
    public override async Task<ActionResult<int>> DeleteAsync([FromRoute] long id)
    {
        try
        {
            var image = await _imageRepository.GetByIdAsync(id);
            if (image is null)
                return NotFound();
            await _imageRepository.DeleteAsync(image);
            return Ok(1);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting image with id {Id}", id);
            return StatusCode(500);
        }
    }
}