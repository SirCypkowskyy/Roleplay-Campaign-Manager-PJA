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
    public Image CharacterImage { get; set; }

    /// <summary>
    /// Id zdjęcia postaci
    /// </summary>
    public long CharacterImageId { get; set; }

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
    /// Relacje tej postaci z innymi postaciami
    /// </summary>
    public ICollection<CharacterRelationWith> RelationsFromCharacterToOthers { get; set; } =
        new List<CharacterRelationWith>();

    /// <summary>
    /// Relacje innych postaci z tą postacią
    /// </summary>
    public ICollection<CharacterRelationWith> RelationsFromOthersToCharacter { get; set; } =
        new List<CharacterRelationWith>();

    /// <summary>
    /// Statystyki postaci
    /// </summary>
    public ICollection<Stat> Stats { get; set; } = new List<Stat>();

    /// <summary>
    /// Przedmioty, które posiada postać
    /// </summary>
    public ICollection<Item> Items { get; set; } = new List<Item>();

    /// <summary>
    /// Notatki dotyczące postaci
    /// </summary>
    public ICollection<Note> Notes { get; set; } = new List<Note>();
}