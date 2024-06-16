using MasFinalProj.Domain.Models.Campaigns;

namespace MasFinalProj.Domain.Repositories;

/// <summary>
/// Repozytorium wiadomości.
/// </summary>
public interface IMessageRepository : IGenericRepository<long, Message>
{    
    /// <summary>
    /// Pobiera wiadomości dla kampanii.
    /// </summary>
    /// <param name="campaignId">
    /// Id kampanii.
    /// </param>
    /// <param name="numToQuery">
    /// Ilość do pobrania.
    /// </param>
    /// <param name="skip">
    /// Ilość do pominięcia.    
    /// </param>
    /// <returns></returns>
    Task<List<Message>> GetPaginatedMessagesForCampaignAsync(string campaignId, int numToQuery, int skip);
}