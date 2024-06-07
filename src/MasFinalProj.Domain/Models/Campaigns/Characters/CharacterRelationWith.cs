using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;

namespace MasFinalProj.Domain.Models.Campaigns.Characters;

/// <summary>
/// Encja relacji między postaciami.
/// </summary>
public class CharacterRelationWith : BaseEntity<long>
{
    /// <summary>
    /// Postać, która jest właścicielem relacji.
    /// </summary>
    public Character FromCharacter { get; set; }

    /// <summary>
    /// Id postaci, która jest właścicielem relacji.
    /// </summary>
    public long FromCharacterId { get; set; }

    /// <summary>
    /// Postać, do której postać ma jakąś relację.
    /// </summary>
    public Character ToCharacter { get; set; }

    /// <summary>
    /// Id postaci, do której postać ma jakąś relację.
    /// </summary>
    public long ToCharacterId { get; set; }

    /// <summary>
    /// Opis relacji.
    /// </summary>
    [MaxLength(500)]
    public string Description { get; set; }

    /// <summary>
    /// Wartość relacji.
    /// </summary>
    [Range(-100, 100)]
    public decimal? RelationValue { get; set; }
}