using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Data;
using Microsoft.Extensions.Logging;

namespace MasFinalProj.Persistence.Repositories;

/// <summary>
/// Implementacja repozytorium kampanii.
/// </summary>
public class CampaignRepository : GenericRepository<Guid, Campaign>, ICampaignRepository
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GenericRepository<Guid, Campaign>> _logger;
    
    /// <summary>
    /// Konstruktor repozytorium kampanii.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public CampaignRepository(DatabaseContext context, ILogger<GenericRepository<Guid, Campaign>> logger) : base(context, logger)
    {
        _context = context;
        _logger = logger;
    }
}