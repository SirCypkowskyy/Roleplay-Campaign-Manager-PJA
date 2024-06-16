using System.ComponentModel.DataAnnotations;

namespace MasFinalProj.Domain.DTOs.Messages.Input;

/// <summary>
/// DTO wejściowy do wysyłania wiadomości
/// </summary>
public class SendMessageInputDTO
{
    /// <summary>
    /// Wiadomość do wysłania
    /// </summary>
    [MaxLength(5000)]
    public string Message { get; set; }

    /// <summary>
    /// Nazwa postaci
    /// </summary>
    [MaxLength(100)]
    public string CharacterName { get; set; } = string.Empty;

    /// <summary>
    /// Autor wiadomości
    /// </summary>
    [MaxLength(100)]
    public string MessageAuthor { get; set; }
}