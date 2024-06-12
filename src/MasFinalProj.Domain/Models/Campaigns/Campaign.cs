using System.Collections;
using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Models.Campaigns.Characters;
using MasFinalProj.Domain.Models.Campaigns.Users;
using MasFinalProj.Domain.Models.Common;

namespace MasFinalProj.Domain.Models.Campaigns;

/// <summary>
/// Encja kampanii
/// </summary>
public class Campaign : BaseEntity<Guid>, IValidateOnSave
{
    /// <summary>
    /// Nazwa kampanii
    /// </summary>
    [MaxLength(52)]
    public string Name { get; set; }

    /// <summary>
    /// Opis kampanii
    /// </summary>
    [MaxLength(2000)]
    public string Description { get; set; }

    /// <summary>
    /// Zdjęcie kampanii
    /// </summary>
    public Image? CampaignImage { get; set; }

    /// <summary>
    /// Id zdjęcia kampanii
    /// </summary>
    public long? CampaignImageId { get; set; }

    /// <summary>
    /// Czy kampania jest zaarchiwizowana
    /// </summary>
    public bool IsArchived { get; set; } = false;

    /// <summary>
    /// Czy kampania jest publiczna
    /// </summary>
    public bool IsPublic { get; set; } = false;

    /// <summary>
    /// Waluta w grze
    /// </summary>
    [MaxLength(100)]
    public string? GameCurrency { get; set; }
    
    /// <summary>
    /// Postacie w ramach kampanii
    /// </summary>
    public virtual ICollection<Character> Characters { get; set; }
    
    /// <summary>
    /// Gracze w ramach kampanii
    /// </summary>
    public virtual ICollection<CampaignUserPlayer> CampaignPlayers { get; set; } = new List<CampaignUserPlayer>();
    
    /// <summary>
    /// Mistrz gry w ramach kampanii
    /// </summary>
    /// <remarks>
    /// Mistrz gry jest jeden na kampanię, ale z racji na naturę konfiguracji relacji w EF Core,
    /// konieczne jest użycie kolekcji
    /// </remarks>
    public virtual ICollection<CampaignUserGameMaster>  CampaignGameMaster { get; set; }
    
    /// <summary>
    /// Notatki w ramach kampanii
    /// </summary>
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    /// <inheritdoc />
    public void ValidateBeforeSave()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new ValidationException("Nazwa kampanii nie może być pusta");
        
        if(CampaignGameMaster.Count != 1)
            throw new ValidationException("Kampania musi mieć dokładnie jednego mistrza gry");
    }
}