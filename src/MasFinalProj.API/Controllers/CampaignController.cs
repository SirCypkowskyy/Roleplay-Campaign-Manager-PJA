using MasFinalProj.Domain.Repositories;
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
    private readonly ILogger<CampaignController> _logger;
    
    /// <summary>
    /// Konstruktor kontrolera kampanii.
    /// </summary>
    /// <param name="campaignRepository">
    /// Repozytorium kampanii.
    /// </param>
    /// <param name="logger">
    /// Logger.
    /// </param>
    public CampaignController(ICampaignRepository campaignRepository, ILogger<CampaignController> logger)
    {
        _campaignRepository = campaignRepository;
        _logger = logger;
    }
    
}