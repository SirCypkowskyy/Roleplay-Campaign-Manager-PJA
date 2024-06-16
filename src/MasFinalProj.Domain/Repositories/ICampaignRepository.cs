using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Models.Campaigns.Characters;
using MasFinalProj.Domain.Models.Campaigns.Users;

namespace MasFinalProj.Domain.Repositories;

/// <summary>
/// Interfejs repozytorium kampanii.
/// </summary>
public interface ICampaignRepository : IGenericRepository<Guid, Campaign>
{
    /// <summary>
    /// Pobiera kampanie gracza.
    /// </summary>
    /// <param name="username">
    /// Nazwa użytkownika gracza.
    /// </param>
    /// <returns>
    /// Lista kampanii gracza.
    /// </returns>
    Task<List<Campaign>> GetPlayerCampaignsAsync(string username);

    /// <summary>
    /// Pobiera kampanię z graczami.
    /// </summary>
    /// <param name="campaignId">
    /// Id kampanii.
    /// </param>
    /// <param name="asNoTracking">
    /// Czy zwrócić obiekt bez śledzenia.
    /// </param>
    /// <returns></returns>
    Task<Campaign?> GetWithPlayersAsync(string campaignId, bool asNoTracking = false);
    
    /// <summary>
    /// Pobiera postać z kampanii.
    /// </summary>
    /// <param name="campaignId">
    /// Id kampanii.
    /// </param>
    /// <param name="characterName">
    /// Nazwa postaci.
    /// </param>
    /// <returns></returns>
    Task<Character?> GetCharacterByNameFromCampaignAsync(string campaignId, string characterName);
    
    /// <summary>
    /// Pobiera użytkownika kampanii.
    /// </summary>
    /// <param name="campaignId">
    /// Id kampanii.
    /// </param>
    /// <param name="username">
    /// Nazwa użytkownika.
    /// </param>
    /// <returns></returns>
    Task<CampaignUser?> GetCampaignUserAsync(string campaignId, string username);

    /// <summary>
    /// Pobiera postacie kontrolowalne przez gracza.
    /// </summary>
    /// <param name="campaignId"></param>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<List<Character>> GetControllableCharactersAsync(string campaignId, string? username);

    /// <summary>
    /// Sprawdza, czy użytkownik może zobaczyć chat kampanii.
    /// </summary>
    /// <param name="campaignId">
    /// Id kampanii.
    /// </param>
    /// <param name="userId">
    /// Id użytkownika.
    /// </param>
    /// <returns></returns>
    Task<bool> CanUserSeeChatAsync(string campaignId, Guid userId);
}