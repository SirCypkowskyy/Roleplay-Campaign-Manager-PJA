using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Models.Campaigns.Characters;
using MasFinalProj.Domain.Models.Campaigns.Users;

namespace MasFinalProj.Domain.Models.Campaigns;

/// <summary>
/// Encja wiadomości w ramach kampanii
/// </summary>
public class Message : BaseEntity<long>
{
    /// <summary>
    /// Zawartość wiadomości
    /// </summary>
    [MaxLength(5000)]
    public string Content { get; set; }

    /// <summary>
    /// Kampania, w ramach której została wysłana wiadomość
    /// </summary>
    public Campaign Campaign { get; set; }

    /// <summary>
    /// Id kampanii, w ramach której została wysłana wiadomość
    /// </summary>
    public Guid CampaignId { get; set; }

    /// <summary>
    /// Postać, która przekazała wiadomość
    /// </summary>
    /// <remarks>
    /// Postać może nie być przypisana, jeśli wiadomość została przekazana przez narratora / system
    /// </remarks>
    public Character? CharacterAuthor { get; set; }

    /// <summary>
    /// Id postaci, która przekazała wiadomość
    /// </summary>
    /// <remarks>
    /// Postać może nie być przypisana, jeśli wiadomość została przekazana przez narratora / system
    /// </remarks>
    public long? CharacterAuthorId { get; set; }

    /// <summary>
    /// Ludzki autor wiadomości
    /// </summary>
    public CampaignUser Author { get; set; }

    /// <summary>
    /// Id ludzkiego autora wiadomości
    /// </summary>
    public long AuthorId { get; set; }
}