using System.Collections;
using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Models.Campaigns.Users;
using MasFinalProj.Domain.Models.Common;
using MasFinalProj.Domain.Models.Users.New;

namespace MasFinalProj.Domain.Models.Users;

/// <summary>
/// Encja użytkownika aplikacji
/// </summary>
public class User : BaseEntity<Guid>
{
    /// <summary>
    /// Nazwa użytkownika
    /// </summary>
    /// <remarks>
    /// Atrybut unikalny
    /// </remarks>
    [MaxLength(32)]
    public string Username { get; set; }

    /// <summary>
    /// Email użytkownika
    /// </summary>
    /// <remarks>
    /// Atrybut unikalny
    /// </remarks>
    [MaxLength(52)]
    [EmailAddress]
    public string Email { get; set; }
    
    /// <summary>
    /// Id użytkownika na platformie Discord
    /// </summary>
    /// <remarks>
    /// Może być null, jeśli użytkownik nigdy nie zalogował się przez Discord i zarejestrował się tradycyjnie
    /// </remarks>
    public long? DiscordId { get; set; }
    
    /// <summary>
    /// Nazwa użytkownika na platformie Discord
    /// </summary>
    /// <remarks>
    /// Może być null w przypadku, gdy użytkownik nigdy nie zalogował się przez Discord i zarejestrował się tradycyjnie
    /// </remarks>
    public string? DiscordUsername { get; set; }

    /// <summary>
    /// Hasło użytkownika
    /// </summary>
    /// <remarks>
    /// Przechowywane w formacie base64
    /// </remarks>
    [MaxLength(64)]
    public string PasswordHash { get; set; }

    /// <summary>
    /// Sól hasła
    /// </summary>
    /// <remarks>
    /// Przechowywana w formacie base64
    /// </remarks>
    [MaxLength(64)]
    public string PasswordSalt { get; set; }

    /// <summary>
    /// Opis użytkownika
    /// </summary>
    /// <remarks>
    /// Atrybut opcjonalny
    /// </remarks>
    [MaxLength(2500)]
    public string? Description { get; set; }

    /// <summary>
    /// Zdjęcie profilowe użytkownika
    /// </summary>
    public Image? ProfileImage { get; set; }

    /// <summary>
    /// Id zdjęcia profilowego
    /// </summary>
    public long? ProfileImageId { get; set; }

    /// <summary>
    /// Zwraca informację, czy użytkownik jest aktywny
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Kampanie, w których uczestniczy użytkownik jako gracz
    /// </summary>
    public virtual ICollection<CampaignUserPlayer> CampaignsAsPlayer { get; set; } = new List<CampaignUserPlayer>();
    
    /// <summary>
    /// Kampanie, w których uczestniczy użytkownik jako mistrz gry
    /// </summary>
    public virtual ICollection<CampaignUserGameMaster> CampaignsAsGameMaster { get; set; } = new List<CampaignUserGameMaster>();
 
    /// <summary>
    /// Odświeżające tokeny użytkownika
    /// </summary>
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}