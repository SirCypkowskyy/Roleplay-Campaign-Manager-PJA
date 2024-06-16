using MasFinalProj.Domain.Models.Campaigns;

namespace MasFinalProj.Domain.DTOs.Messages.Output;

/// <summary>
/// DTO wiadomości w ramach kampanii.
/// </summary>
public class MessageResponseDTO
{
    /// <summary>
    /// Treść wiadomości.
    /// </summary>
    public string Text { get; set; }

    /// <summary>
    /// Autor wiadomości.
    /// </summary>
    public string Sender { get; set; }

    /// <summary>
    /// Postać autora wiadomości.
    /// </summary>
    public string Character { get; set; }

    /// <summary>
    /// Czas wysłania wiadomości.
    /// </summary>
    public DateTime Time { get; set; }

    /// <summary>
    /// Id obrazka.
    /// </summary>
    public string? ImageId { get; set; }
    
    public MessageResponseDTO() {}
    
    public MessageResponseDTO(string text, string sender, string character, DateTime time)
    {
        Text = text;
        Sender = sender;
        Character = character;
        Time = time;
    }
    
    public MessageResponseDTO(Message message)
    {
        Text = message.Content;
        Sender = message.Author.User.Username;
        Character = message.CharacterAuthor?.Name ?? "";
        Time = message.CreatedAtUtc;
        ImageId = message.CharacterAuthor?.CharacterImageId.ToString();
    }
}