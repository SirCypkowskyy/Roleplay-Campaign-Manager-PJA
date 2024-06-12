using MasFinalProj.Domain.Models.Campaigns;

namespace MasFinalProj.Domain.Repositories;

/// <summary>
/// Interfejs repozytorium kampanii.
/// </summary>
public interface ICampaignRepository : IGenericRepository<Guid, Campaign>
{
    
}