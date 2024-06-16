using MasFinalProj.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MasFinalProj.API.Controllers;

/// <summary>
/// Kontroler wiadomości.
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;
    private readonly IMessageRepository _messageRepository;
    
    /// <summary>
    /// Konstruktor kontrolera wiadomości.
    /// </summary>
    /// <param name="logger">
    /// Logger.
    /// </param>
    /// <param name="messageRepository">
    /// Repozytorium wiadomości.
    /// </param>
    public MessageController(ILogger<MessageController> logger, IMessageRepository messageRepository)
    {
        _logger = logger;
        _messageRepository = messageRepository;
    }
    
    
 
}