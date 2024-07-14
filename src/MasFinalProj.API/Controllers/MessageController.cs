/* @2024 Cyprian Gburek.
 * Proszę nie kopiować kodu bez zgody autora.
 * Kod jest własnością uczelni (Polsko-Japońska Akademia Technik Komputerowych, PJATK),
 * i jest udostępniany wyłącznie w celach edukacyjnych.
 *
 * Wykorzystanie kodu we własnych projektach na zajęciach jest zabronione, a jego wykorzystanie
 * może skutkować oznanieniem projektu jako plagiat.
 */
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