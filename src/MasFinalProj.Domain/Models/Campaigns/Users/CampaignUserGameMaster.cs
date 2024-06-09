using System.ComponentModel.DataAnnotations;

namespace MasFinalProj.Domain.Models.Campaigns.Users;

/// <summary>
/// Klasa użytkownika kampanii
/// </summary>
public class CampaignUserGameMaster : CampaignUser
{
    /// <summary>
    /// Personalny opis przebiegu kampanii
    /// </summary>
    [MaxLength(5000)]
    public string? Storyline { get; set; }
}