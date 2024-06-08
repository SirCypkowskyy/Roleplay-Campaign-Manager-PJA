using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Models.Campaigns.Characters;
using MasFinalProj.Domain.Models.Users;

namespace MasFinalProj.Domain.Models.Common;

/// <summary>
/// Encja zdjęcia
/// </summary>
public class Image : BaseEntity<long>
{
    /// <summary>
    /// Nazwa zdjęcia
    /// </summary>
    public string ImageName { get; set; }

    /// <summary>
    /// Format zdjęcia
    /// </summary>
    [MaxLength(5)]
    public string ImageFormat { get; set; }

    /// <summary>
    /// Zdjęcie w formacie Base64
    /// </summary>
    public string Base64Image { get; set; }
    
    /// <summary>
    /// Suma kontrolna zdjęcia (MD5)
    /// </summary>
    public string Checksum { get; set; }
    
    /// <summary>
    /// Użytkownicy, którzy posiadają to zdjęcie
    /// </summary>
    public virtual ICollection<User> UsersWithImage { get; set; }
    
    /// <summary>
    /// Kampanie, do których przypisane jest zdjęcie
    /// </summary>
    public virtual ICollection<Campaign> CampaignsWithImage { get; set; }
    
    /// <summary>
    /// Postacie, do których przypisane jest zdjęcie
    /// </summary>
    public virtual ICollection<Character> Characters { get; set; }
}