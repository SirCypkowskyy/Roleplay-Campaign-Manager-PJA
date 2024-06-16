using MasFinalProj.Domain.Models.Common;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Data;
using Microsoft.Extensions.Logging;

namespace MasFinalProj.Persistence.Repositories;

/// <summary>
/// Repozytorium obrazów
/// </summary>
public class ImageRepository : GenericRepository<long, Image>, IImageRepository
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GenericRepository<long, Image>> _logger;
    
    public ImageRepository(DatabaseContext context, ILogger<GenericRepository<long, Image>> logger) : base(context, logger)
    {
        _context = context;
        _logger = logger;
    }
}