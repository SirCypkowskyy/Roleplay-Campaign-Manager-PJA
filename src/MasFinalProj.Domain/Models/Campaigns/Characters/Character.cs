using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Models.Campaigns.Users;
using MasFinalProj.Domain.Models.Common;

namespace MasFinalProj.Domain.Models.Campaigns.Characters;

/// <summary>
/// Encja postaci
/// </summary>
public class Character : BaseEntity<long>
{
    /// <summary>
    /// Imię postaci
    /// </summary>
    [MaxLength(120)]
    public string Name { get; set; }

    /// <summary>
    /// Opis postaci
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Biografia postaci
    /// </summary>
    [MaxLength(2500)]
    public string? Bio { get; set; }

    /// <summary>
    /// Zdjęcie postaci
    /// </summary>
    public Image? CharacterImage { get; set; }

    /// <summary>
    /// Id zdjęcia postaci
    /// </summary>
    public long? CharacterImageId { get; set; }

    /// <summary>
    /// Pieniądze postaci
    /// </summary>
    public decimal Money { get; set; } = 0;

    /// <summary>
    /// Gracz, który jest właścicielem postaci
    /// </summary>
    /// <remarks>
    /// Postać może nie mieć właściciela, jeśli jest to postać NPC
    /// </remarks>
    public CampaignUserPlayer? PlayerOwner { get; set; }

    /// <summary>
    /// Id gracza, który jest właścicielem postaci
    /// </summary>
    /// <remarks>
    /// Postać może nie mieć właściciela, jeśli jest to postać NPC
    /// </remarks>
    public long? PlayerOwnerId { get; set; }
    
    /// <summary>
    /// Kampania, w której znajduje się postać
    /// </summary>
    public Campaign Campaign { get; set; }
    
    /// <summary>
    /// Id kampanii, w której znajduje się postać
    /// </summary>
    public Guid CampaignId { get; set; }
    
    /// <summary>
    /// Wiadomości, które postać wysłała
    /// </summary>
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    
    /// <summary>
    /// Relacje tej postaci z innymi postaciami
    /// </summary>
    public virtual ICollection<CharacterRelationWith> RelationsFromCharacterToOthers { get; set; } =
        new List<CharacterRelationWith>();

    /// <summary>
    /// Relacje innych postaci z tą postacią
    /// </summary>
    public virtual ICollection<CharacterRelationWith> RelationsFromOthersToCharacter { get; set; } =
        new List<CharacterRelationWith>();

    /// <summary>
    /// Atrybuty postaci
    /// </summary>
    public virtual ICollection<CharacterAttribute> CharacterAttributes { get; set; } = new List<CharacterAttribute>();

    /// <summary>
    /// Notatki dotyczące postaci
    /// </summary>
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();

    /// <summary>
    /// Zwraca sumę pieniędzy postaci oraz wartości jej ekwipunku
    /// </summary>
    /// <returns></returns>
    public decimal GetTotalMoney()
    {
        var characterItemAttributes = CharacterAttributes
            .OfType<Item>()
            .Select(i => i.MoneyValue).Sum();
        
        var characterStatItemAttributes = CharacterAttributes
            .OfType<StatItem>()
            .Select(i => i.MoneyValue).Sum();
        
        return Money + characterItemAttributes + characterStatItemAttributes;
    }
}