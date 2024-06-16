

using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasFinalProj.Persistence.Repositories;

/// <summary>
/// Repozytorium wiadomości.
/// </summary>
public class MessageRepository : GenericRepository<long, Message>, IMessageRepository
{
    private readonly DatabaseContext _context;
    private readonly ILogger<GenericRepository<long, Message>> _logger;
    
    /// <summary>
    /// Konstruktor repozytorium wiadomości.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="logger"></param>
    public MessageRepository(DatabaseContext context, ILogger<GenericRepository<long, Message>> logger) : base(context, logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<List<Message>> GetPaginatedMessagesForCampaignAsync(string campaignId, int numToQuery, int skip)
    {
        var messages=  await _context.Messages
            .Include(m => m.Author)
            .Include(m => m.CharacterAuthor)
            .Where(m => m.CampaignId == Guid.Parse(campaignId))
            .OrderByDescending(m => m.CreatedAtUtc)
            .Skip(skip)
            .Take(numToQuery)
            .AsNoTracking()
            .ToListAsync();
        
        var campaignUsers = messages.Select(m => m.Author).Where(a => a is not null)
            .Select(a => a.UserId).Distinct().ToList();
        
        var users = await _context.Users.Where(u => campaignUsers.Contains(u.Id)).ToListAsync();
        foreach (var message in messages)
            message.Author.User = users.First(u => u.Id == message.Author.UserId);
        return messages;
    }

}