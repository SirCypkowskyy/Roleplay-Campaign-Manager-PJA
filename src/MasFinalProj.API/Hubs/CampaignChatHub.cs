using MasFinalProj.Persistence.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

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
        // _connections.Add(Context.UserIdentifier, Context.ConnectionId);
        
        await NewMessage($"{Context.UserIdentifier} joined the chat", "Server", "");
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
        await NewMessage(text, Context.UserIdentifier, character);
    }
}