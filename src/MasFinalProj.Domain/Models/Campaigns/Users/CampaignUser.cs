using System.Collections;
using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Models.Campaigns.Characters;
using MasFinalProj.Domain.Models.Users;

namespace MasFinalProj.Domain.Models.Campaigns.Users;

/// <summary>
/// Abstrakcyjna klasa użytkownika kampanii
/// </summary>
public abstract class CampaignUser : BaseEntity<long>
{
    /// <summary>
    /// Użytkownik
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Id Użytkownika
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Kampania
    /// </summary>
    public Campaign Campaign { get; set; }

    /// <summary>
    /// Id Kampanii
    /// </summary>
    public Guid CampaignId { get; set; }

    /// <summary>
    /// Nickname użytkownika w kampanii
    /// </summary>
    [MaxLength(32)]
    public string? CampaignNickname { get; set; }

    /// <summary>
    /// Bio użytkownika w kampanii
    /// </summary>
    [MaxLength(200)]
    public string? CampaignBio { get; set; }

    /// <summary>
    /// Notatki sporządzone przez użytkownika
    /// </summary>
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    /// <summary>
    /// Wiadomości wysłane przez użytkownika
    /// </summary>
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}