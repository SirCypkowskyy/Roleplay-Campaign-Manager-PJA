/* @2024 Cyprian Gburek.
 * Proszę nie kopiować kodu bez zgody autora.
 * Kod jest własnością uczelni (Polsko-Japońska Akademia Technik Komputerowych, PJATK),
 * i jest udostępniany wyłącznie w celach edukacyjnych.
 *
 * Wykorzystanie kodu we własnych projektach na zajęciach jest zabronione, a jego wykorzystanie
 * może skutkować oznanieniem projektu jako plagiat.
 */
using System.IdentityModel.Tokens.Jwt;
using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Models.Users;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace MasFinalProj.API.Hubs;

/// <summary>
/// Hub czatu kampanii.
/// </summary>
public class CampaignChatHub : Hub
{
    private readonly ILogger<CampaignChatHub> _logger;
    private readonly IMessageRepository _messageRepository;
    private readonly ICampaignRepository _campaignRepository;
    private readonly IUserRepository _userRepository;
    private readonly DatabaseContext _dbContext;
    private static readonly Dictionary<string, (string username, string campaignId)> Connections = new();
    
    public CampaignChatHub(ILogger<CampaignChatHub> logger, DatabaseContext dbContext, IMessageRepository messageRepository, ICampaignRepository campaignRepository, IUserRepository userRepository)
    {
        _logger = logger;
        _dbContext = dbContext;
        _messageRepository = messageRepository;
        _campaignRepository = campaignRepository;
        _userRepository = userRepository;
    }
    
    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("User connected: {ConnectionId}", Context.ConnectionId);
        
        var token = Context.GetHttpContext()?.Request.Cookies["token"];
        
        if (token is null)
        {
            _logger.LogWarning("Token not found");
            Context.Abort();
            return;
        }
        
        var user = await GetUserWithJwtToken(token);
        if (user is null)
        {
            _logger.LogWarning("User not found");
            Context.Abort();
            return;
        }
        
        _logger.LogInformation("User found: {UserId}", user.Id);
        
        var campaignId = Context.GetHttpContext().Request.Query["campaignId"].ToString();
        if (string.IsNullOrEmpty(campaignId))
        {
            _logger.LogWarning("Campaign ID not provided");
            Context.Abort();
            return;
        }
        
        var canSeeChat = await _campaignRepository.CanUserSeeChatAsync(campaignId, user.Id);
        
        if (!canSeeChat)
        {
            _logger.LogWarning("User cannot see chat");
            Context.Abort();
            return;
        }

        Connections[Context.ConnectionId] = (user.Username, campaignId);
        await Groups.AddToGroupAsync(Context.ConnectionId, campaignId);
        await NewMessage($"{user.Username} joined the chat", "System", "", campaignId);
    }

    private async Task<User?> GetUserWithJwtToken(string token)
    {
        var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var userId = decodedToken.Claims.First(c => c.Type == "uid").Value;
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
    }
    
    /// <summary>
    /// Wysyła nową wiadomość do klientów w danej kampanii.
    /// </summary>
    /// <param name="sender">
    /// Użytkownik, który wysłał wiadomość.
    /// </param>
    /// <param name="text">
    /// Wiadomość.
    /// </param>
    /// <param name="character">
    /// Postać, z której wysłano wiadomość.
    /// </param>
    /// <param name="campaignId">
    /// Identyfikator kampanii.
    /// </param>
    public async Task NewMessage(string text, string sender, string character, string campaignId)
    {
        await Clients.Group(campaignId).SendAsync("ReceiveMessage", text, sender, character, DateTime.UtcNow);
        var characterAuthor = await _campaignRepository.GetCharacterByNameFromCampaignAsync(campaignId, character);
        
        if(sender == "System")
            return;
        
        if (characterAuthor != null)
        {
            await _messageRepository.AddAsync(new Message
            {
                Content = text,
                CampaignId = Guid.Parse(campaignId),
                AuthorId = (await _campaignRepository.GetCampaignUserAsync(campaignId, sender))!.Id,
                CharacterAuthorId = characterAuthor.Id,
            });
        }
        else
        {
            var author = await _campaignRepository.GetCampaignUserAsync(campaignId, sender);
            if (author is null)
            {
                _logger.LogWarning("Author not found");
                return;
            }
            await _messageRepository.AddAsync(new Message
            {
                Content = text,
                CampaignId = Guid.Parse(campaignId),
                AuthorId = (await _campaignRepository.GetCampaignUserAsync(campaignId, sender))!.Id,
            });
        }
       
    }
    
    public async Task GetMessages(string campaignId, int numToQuery, int skip)
    {
        _logger.LogInformation("Getting messages for campaign {CampaignId}", campaignId);
        var messages = await _messageRepository.GetPaginatedMessagesForCampaignAsync(campaignId, numToQuery, skip);
        await Clients.Caller.SendAsync("ReceiveMessages", messages.Select(m => new
        {
            m.Content,
            m.Author.User.Username,
            m.CharacterAuthor?.Name,
            m.CreatedAtUtc,
        }).ToList());
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("User disconnected: {ConnectionId}", Context.ConnectionId);

        if (Connections.TryGetValue(Context.ConnectionId, out var userCampaignInfo))
        {
            var (username, campaignId) = userCampaignInfo;
            await NewMessage($"{username} left the chat", "System", "", campaignId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, campaignId);
            Connections.Remove(Context.ConnectionId);
        }
    }
    
    /// <summary>
    /// Wysyła wiadomość do klientów w danej kampanii.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="character"></param>
    public async Task SendMessage(string text, string character)
    {
        var token = Context.GetHttpContext().Request.Cookies["token"];
        
        var user = await GetUserWithJwtToken(token);
        if (user is null)
        {
            _logger.LogWarning("User not found");
            return;
        }

        var campaignId = Context.GetHttpContext().Request.Query["campaignId"].ToString();
        if (string.IsNullOrEmpty(campaignId))
        {
            _logger.LogWarning("Campaign ID not provided");
            return;
        }
        
        await NewMessage(text, user.Username, character, campaignId);
    }
}
