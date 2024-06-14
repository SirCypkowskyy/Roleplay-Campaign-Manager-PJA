using System.IdentityModel.Tokens.Jwt;
using MasFinalProj.Domain.Models.Users;
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
    private readonly DatabaseContext _dbContext;
    // private readonly IDictionary<string, string> _connections = new Dictionary<string, string>();
    
    public CampaignChatHub(ILogger<CampaignChatHub> logger, DatabaseContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }
    
    /// <inheritdoc />
    public override async Task OnConnectedAsync()
    {
        _logger.LogInformation("User connected: {ConnectionId}", Context.ConnectionId);
        
        var token = Context.GetHttpContext().Request.Cookies["token"];
        
        var user = await GetUserWithJwtToken(token);
        if (user is null)
        {
            _logger.LogWarning("User not found");
            return;
        }
        
        _logger.LogInformation("User found: {UserId}", user.Id);
        
        
        await NewMessage($"{user.Username} joined the chat", "System", "");
    }

    private async Task<User?> GetUserWithJwtToken(string token)
    {
        var decodedToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var userId = decodedToken.Claims.First(c => c.Type == "uid").Value;
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
    }
    
    /// <summary>
    /// Wysyła nową wiadomość do wszystkich klientów.
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
    public async Task NewMessage(string text, string sender, string character)
    {
        await Clients.All.SendAsync("ReceiveMessage", text, sender, character, DateTime.UtcNow);
    }

    /// <inheritdoc />
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _logger.LogInformation("User disconnected: {ConnectionId}", Context.ConnectionId);
        // _connections.Remove(Context.UserIdentifier);
        
        await NewMessage($"{Context.UserIdentifier} left the chat", "Server", "");
    }
    
    /// <summary>
    /// Wysyła wiadomość do wszystkich klientów.
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
        
        await NewMessage(text, user.Username, character);
    }
}