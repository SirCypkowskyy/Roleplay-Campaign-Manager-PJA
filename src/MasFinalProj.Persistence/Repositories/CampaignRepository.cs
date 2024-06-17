using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Models.Campaigns.Characters;
using MasFinalProj.Domain.Models.Campaigns.Users;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasFinalProj.Persistence.Repositories;

/// <summary>
/// Implementacja repozytorium kampanii.
/// </summary>
public class CampaignRepository : GenericRepository<Guid, Campaign>, ICampaignRepository
{
    private readonly DatabaseContext _context;
    private readonly ILogger<CampaignRepository> _logger;
    
    /// <summary>
    /// Konstruktor repozytorium kampanii.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public CampaignRepository(DatabaseContext context, ILogger<CampaignRepository> logger) : base(context, logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<List<Campaign>> GetPlayerCampaignsAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user is null)
        {
            _logger.LogWarning("User with username {Username} not found", username);
            return [];
        }
        
        var campaignsAsPlayer = await _context.Campaigns.Include(c => c.CampaignPlayers)
            .Include(c => c.CampaignPlayers).ThenInclude(cp => cp.User)
            .Include(c => c.CampaignGameMaster).ThenInclude(cgm => cgm.User)
            .Where(c => c.CampaignPlayers.Any(cp => cp.UserId == user.Id))
            .AsNoTracking()
            .ToListAsync();
        
        _logger.LogInformation("Found {Count} campaigns as player", campaignsAsPlayer.Count);
        
        var campaignsAsGameMaster = await _context.Campaigns.Include(c => c.CampaignGameMaster)
            .Include(c => c.CampaignGameMaster).ThenInclude(cgm => cgm.User)
            .Include(c => c.CampaignPlayers).ThenInclude(cp => cp.User)
            .Where(c => c.CampaignGameMaster.Any(cgm => cgm.UserId == user.Id))
            .AsNoTracking()
            .ToListAsync();
        
        _logger.LogInformation("Found {Count} campaigns as game master", campaignsAsGameMaster.Count);
        
        return campaignsAsPlayer.Concat(campaignsAsGameMaster).ToList();
        
        // return await _context.Campaigns.Include(c => c.CampaignPlayers).ThenInclude(cp => cp.User)
        //     .Include(c => c.CampaignImage)
        //     .Include(c => c.CampaignGameMaster).ThenInclude(cgm => cgm.User)
        //     .Where(c => c.CampaignPlayers.Any(cp => cp.User.Username == username) ||
        //                 c.CampaignGameMaster.Any(cgm => cgm.User.Username == username))
        //     .AsNoTracking()
        //     .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Campaign?> GetWithPlayersAsync(string campaignId, bool asNoTracking = false)
    {
        if(asNoTracking)
            return await _context.Campaigns.Include(c => c.CampaignPlayers).ThenInclude(cp => cp.User)
                .Include(c => c.CampaignImage)
                .Include(c => c.CampaignGameMaster).ThenInclude(cgm => cgm.User)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id.ToString() == campaignId);
        
        return await _context.Campaigns.Include(c => c.CampaignPlayers).ThenInclude(cp => cp.User)
            .Include(c => c.CampaignImage)
            .Include(c => c.CampaignGameMaster).ThenInclude(cgm => cgm.User)
            .FirstOrDefaultAsync(c => c.Id.ToString() == campaignId);
    }

    /// <inheritdoc />
    public async Task<Character?> GetCharacterByNameFromCampaignAsync(string campaignId, string characterName)
    {
        return await _context.Characters.Include(c => c.Campaign)
            .FirstOrDefaultAsync(c => c.Campaign.Id.ToString() == campaignId && c.Name == characterName);
    }

    /// <inheritdoc />
    public async Task<CampaignUser?> GetCampaignUserAsync(string campaignId, string username)
    {
        var campaign = await _context.Campaigns.FirstOrDefaultAsync(c => c.Id.ToString() == campaignId);
        if (campaign is null)
        {
            _logger.LogWarning("Campaign not found");
            return null;
        }
        
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        
        if(user is null)
        {
            _logger.LogWarning("User with username {Username} not found", username);
            return null;
        }
        
        return await _context.CampaignUsers.FirstOrDefaultAsync(cu => cu.CampaignId == campaign.Id && cu.UserId == user.Id);
    }

    /// <summary>
    /// Pobiera postacie kontrolowalne przez gracza.
    /// </summary>
    /// <param name="campaignId">
    /// Id kampanii.
    /// </param>
    /// <param name="username">
    /// Nazwa użytkownika gracza.
    /// </param>
    /// <returns></returns>
    public async Task<List<Character>> GetControllableCharactersAsync(string campaignId, string? username)
    {
        var campaign = await _context.Campaigns.Include(c => c.Characters)
            .Include(c => c.CampaignPlayers)
            .ThenInclude(cp => cp.User)
            .Include(campaign => campaign.CampaignGameMaster)
            .ThenInclude(campaignUserGameMaster => campaignUserGameMaster.User)
            .FirstOrDefaultAsync(c => c.Id.ToString() == campaignId);
        if (campaign is null)
        {
            _logger.LogWarning("Campaign not found");
            return [];
        }
        
        var isUserGameMaster = campaign.CampaignGameMaster.Any(cgm => cgm.User.Username == username);
        if (isUserGameMaster)
            return campaign.Characters.ToList();
        
        var user = campaign.CampaignPlayers.FirstOrDefault(cp => cp.User.Username == username);
        
        if (user is not null) return campaign.Characters.Where(c => c.PlayerOwnerId == user.Id).ToList();
        
        _logger.LogWarning("User not found in campaign");
        return [];

    }

    public async Task<bool> CanUserSeeChatAsync(string campaignId, Guid userId)
    {
        var campaign = await _context.Campaigns.Include(c => c.CampaignPlayers)
            .ThenInclude(cp => cp.User)
            .Include(c => c.CampaignGameMaster)
            .ThenInclude(cgm => cgm.User)
            .FirstOrDefaultAsync(c => c.Id.ToString() == campaignId);

        if (campaign is null)
            return false;
        
        return campaign.CampaignPlayers.Any(cp => cp.User.Id == userId) ||
               campaign.CampaignGameMaster.Any(cgm => cgm.User.Id == userId);
    }
}