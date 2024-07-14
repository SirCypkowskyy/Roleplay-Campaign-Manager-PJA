/* @2024 Cyprian Gburek.
 * Proszę nie kopiować kodu bez zgody autora.
 * Kod jest własnością uczelni (Polsko-Japońska Akademia Technik Komputerowych, PJATK),
 * i jest udostępniany wyłącznie w celach edukacyjnych.
 *
 * Wykorzystanie kodu we własnych projektach na zajęciach jest zabronione, a jego wykorzystanie
 * może skutkować oznanieniem projektu jako plagiat.
 */
using System.Security.Claims;
using MasFinalProj.API.Abstractions;
using MasFinalProj.Domain.DTOs;
using MasFinalProj.Domain.DTOs.Campaigns.Output;
using MasFinalProj.Domain.DTOs.Characters.Output;
using MasFinalProj.Domain.DTOs.Messages.Output;
using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Models.Campaigns.Users;
using MasFinalProj.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasFinalProj.API.Controllers;

/// <summary>
/// Kontroler kampanii.
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class CampaignController : AbstractController<Guid, Campaign, AbstractOutputDTOs.CampaignDto, CampaignResultDTO>
{
    
    private readonly IMessageRepository _messageRepository;
    private readonly ILogger<CampaignController> _logger;
    private readonly ICampaignRepository _campaignRepository;
    private readonly IUserRepository _userRepository;
    
    /// <summary>
    /// Konstruktor kontrolera kampanii.
    /// </summary>
    /// <param name="logger">
    /// Logger.
    /// </param>
    /// <param name="userRepository">
    /// Repozytorium użytkowników.
    /// </param>
    /// <param name="campaignRepository">
    /// Repozytorium kampanii.
    /// </param>
    /// <param name="messageRepository">
    /// Repozytorium wiadomości.
    /// </param>
    public CampaignController(
        ILogger<CampaignController> logger, IUserRepository userRepository, ICampaignRepository campaignRepository, IMessageRepository messageRepository) : base(campaignRepository)
    {
        
        _userRepository = userRepository;
        _messageRepository = messageRepository;
        _logger = logger;
        _campaignRepository = campaignRepository;
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
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CampaignResultDTO))]
    public async Task<IActionResult> GetCampaignWithPlayersAsync([FromRoute] string campaignId)
    {
        try
        {
            var campaign = await _campaignRepository.GetWithPlayersAsync(campaignId);
            if (campaign is null)
                return NotFound();
            
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
    [HttpGet("player/campaings")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CampaignResultDTO>))]
    public async Task<IActionResult> GetPlayerCampaignsAsync([FromQuery] string username)
    {
        try
        {
            var callerIdentity = User.Identity?.Name;
            var callerRole = User.FindFirst(ClaimTypes.Role)?.Value;
            _logger.LogInformation("Caller identity: {CallerIdentity}, caller role: {CallerRole}", callerIdentity, callerRole);
            if (callerIdentity == null)
                return Unauthorized();
            
            if(callerIdentity != username && callerRole != "Admin" && callerRole != "Moderator")
                return Forbid();
            
            var campaigns = await _campaignRepository.GetPlayerCampaignsAsync(username);
            
            if (campaigns.Count == 0)
                return NotFound();
            
            var convertedCampaigns = campaigns
                .Select(c =>  new CampaignResultDTO(c))
                .ToList();
            _logger.LogInformation("Returning campaings with Ids: {CampaignIds}", string.Join(", ", convertedCampaigns.Select(c => c.Id)));
            return Ok(convertedCampaigns);
            
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while getting campaigns for user {Username}", username);
            return StatusCode(500, "Internal server error");
        }
    }
    
    /// <summary>
    /// Pobiera kampanię po id.
    /// </summary>
    /// <param name="id">
    /// Id kampanii.
    /// </param>
    /// <returns>
    /// Kampania.
    /// </returns>
    [HttpGet("generic/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CampaignResultDTO))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public override async Task<ActionResult<CampaignResultDTO>> GetByIdAsync([FromRoute] Guid id)
    {
        try
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);
            if (campaign is null)
                return NotFound();
            var campaignResult = new CampaignResultDTO(campaign);
            return Ok(campaignResult);
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while getting campaign by id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }
    
    /// <summary>
    /// Aktualizuje kampanię.
    /// </summary>
    /// <param name="entity">
    /// Kampania do aktualizacji.
    /// </param>
    /// <returns>
    /// Zaktualizowana kampania.
    /// </returns>
    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CampaignResultDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public override async Task<ActionResult<CampaignResultDTO>> UpdateAsync([FromBody] Campaign entity)
    {
        try
        {
            var updatedCampaign = await _campaignRepository.UpdateAsync(entity);
            var campaignResult = new CampaignResultDTO(updatedCampaign);
            return Ok(campaignResult);
            
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while updating campaign");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Usuwa kampanię.
    /// </summary>
    /// <param name="id">
    /// Id kampanii.
    /// </param>
    /// <returns>
    /// Ilość usuniętych kampanii.
    /// </returns>
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public override async Task<ActionResult<int>> DeleteAsync([FromRoute] Guid id)
    {
        try
        {
            var campaign = await _campaignRepository.GetByIdAsync(id);
            if (campaign is null)
                return NotFound();
            
            await _campaignRepository.DeleteAsync(campaign);
            return Ok(1);
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting campaign by id {Id}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Tworzy kampanię.
    /// </summary>
    /// <param name="entity">
    /// Kampania do utworzenia.
    /// </param>
    /// <returns>
    /// Utworzona kampania.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CampaignResultDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [Authorize]
    public override async Task<ActionResult<CampaignResultDTO>> CreateAsync([FromBody] AbstractOutputDTOs.CampaignDto entity)
    {
        try
        {
            var username = User.Identity?.Name;
            if (username is null)
                return Unauthorized();
            
            var user = await _userRepository.FirstOrDefaultAsync(u => u.Username == username);
            if (user is null)
                return BadRequest();
            
            var campaign = new Campaign()
            {
                Name = entity.Name,
                Description = entity.Description,
                CampaignImageId = entity.ImageId,
                CampaignGameMaster = new List<CampaignUserGameMaster>()
                {
                    new CampaignUserGameMaster()
                    {
                        UserId = user.Id
                    }
                }
            };
            var createdCampaign = await _campaignRepository.AddAsync(campaign);
            var campaignResult = new CampaignResultDTO(createdCampaign);
            return Ok(campaignResult);
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while creating campaign");
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

    /// <summary>
    /// Pobiera wszystkie kampanie.
    /// </summary>
    /// <returns>
    /// Wszystkie kampanie.
    /// </returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CampaignResultDTO>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    [HttpGet]
    public override async Task<ActionResult<List<CampaignResultDTO>>> GetAllAsync()
    {
        try
        {
            var campaigns = await _campaignRepository.GetAllAsync();
            var convertedCampaigns = campaigns.Select(c => new CampaignResultDTO(c)).ToList();
            return Ok(convertedCampaigns);
        } catch (Exception e)
        {
            _logger.LogError(e, "Error while getting all campaigns");
            return StatusCode(500, "Internal server error");
        }
    }

    

    
}