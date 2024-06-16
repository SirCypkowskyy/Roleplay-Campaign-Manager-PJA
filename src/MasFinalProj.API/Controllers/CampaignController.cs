using System.Security.Claims;
using MasFinalProj.Domain.DTOs.Campaigns.Output;
using MasFinalProj.Domain.DTOs.Characters.Output;
using MasFinalProj.Domain.DTOs.Messages.Output;
using MasFinalProj.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasFinalProj.API.Controllers;

/// <summary>
/// Kontroler kampanii.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class CampaignController : ControllerBase
{
    private readonly ICampaignRepository _campaignRepository;
    private readonly IMessageRepository _messageRepository;
    private readonly ILogger<CampaignController> _logger;
    
    /// <summary>
    /// Konstruktor kontrolera kampanii.
    /// </summary>
    /// <param name="campaignRepository">
    /// Repozytorium kampanii.
    /// </param>
    /// <param name="messageRepository">
    /// Repozytorium wiadomości.
    /// </param>
    /// <param name="logger">
    /// Logger.
    /// </param>
    public CampaignController(ICampaignRepository campaignRepository, IMessageRepository messageRepository, ILogger<CampaignController> logger)
    {
        _campaignRepository = campaignRepository;
        _messageRepository = messageRepository;
        _logger = logger;
    }
    
    
    /// <summary>
    /// Pobiera kampanie z graczami.
    /// </summary>
    /// <param name="campaignId">
    /// Id kampanii.
    /// </param>
    /// <returns>
    /// Kampania z graczami.
    /// </returns>
    [HttpGet("{campaignId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CampaignResultDTO))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetCampaignWithPlayersAsync([FromRoute] string campaignId)
    {
        try
        {
            var campaign = await _campaignRepository.GetWithPlayersAsync(campaignId);
            var campaignResult = new CampaignResultDTO(campaign);
            return Ok(campaignResult);
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while getting campaign with players {CampaignId}", campaignId);
            return StatusCode(500, "Internal server error");
        }
    }
    
    
    /// <summary>
    /// Pobiera kampanie gracza.
    /// </summary>
    /// <param name="username">
    /// Nazwa użytkownika gracza.
    /// </param>
    /// <returns></returns>
    [HttpGet("player-campaings")]
    [Authorize]
    public async Task<IActionResult> GetPlayerCampaignsAsync([FromQuery] string username)
    {
        try
        {
            var callerIdentity = User.Identity?.Name;
            var callerRole = User.FindFirst(ClaimTypes.Role)?.Value;
            if (callerIdentity == null)
                return Unauthorized();
            
            if(callerIdentity != username && callerRole != "Admin" && callerRole != "Moderator")
                return Forbid();
            
            var campaigns = await _campaignRepository.GetPlayerCampaignsAsync(username);
            return Ok(campaigns);
            
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while getting campaigns for user {Username}", username);
            return StatusCode(500, "Internal server error");
        }
    }
    
    /// <summary>
    /// Pobiera wiadomości kampanii z paginacją.
    /// </summary>
    /// <param name="campaignId">
    /// Id kampanii.
    /// </param>
    /// <param name="numToQuery">
    /// Ilość wiadomości do pobrania.
    /// </param>
    /// <param name="skip">
    /// Ilość wiadomości do pominięcia.
    /// </param>
    /// <returns>
    /// Wiadomości kampanii.
    /// </returns>
    [HttpGet("{campaignId}/messages")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MessageResponseDTO>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [Authorize]
    public async Task<IActionResult> GetCampaignMessagesPaginatedAsync([FromRoute] string campaignId, [FromQuery] int numToQuery, [FromQuery] int skip)
    {
        try
        {
            var messages = await _messageRepository.GetPaginatedMessagesForCampaignAsync(campaignId, numToQuery, skip);
            
            var convertedMessages = messages.Select(m => new MessageResponseDTO(m)).ToList();
            return Ok(convertedMessages);
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while getting messages for campaign {CampaignId}", campaignId);
            return StatusCode(500, "Internal server error");
        }
    }
    
    /// <summary>
    /// Pobiera postacie, które można kontrolować w kampanii.
    /// </summary>
    /// <param name="campaignId">
    /// Id kampanii.
    /// </param>
    /// <returns></returns>
    [HttpGet("{campaignId}/controllable-characters")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CharacterResponseDTO>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetControllableCampaignCharactersAsync([FromRoute] string campaignId)
    {
        try
        {
            var username = User.Identity?.Name;
            var characters = await _campaignRepository.GetControllableCharactersAsync(campaignId, username);
            var convertedCharacters = characters.Select(c => new CharacterResponseDTO(c)).ToList();
            return Ok(convertedCharacters);
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while getting controllable characters for campaign {CampaignId}", campaignId);
            return StatusCode(500, "Internal server error");
        }
    }
}