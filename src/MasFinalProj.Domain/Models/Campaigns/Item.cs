using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Models.Campaigns.Characters;

namespace MasFinalProj.Domain.Models.Campaigns;

/// <summary>
///     Encja przedmiotu
/// </summary>
/// <remarks>
///     Sortowana po nazwie
/// </remarks>
public class Item : BaseEntity<long>
{
    /// <summary>
    ///     Nazwa przedmiotu
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Opis przedmiotu
    /// </summary>
    /// <remarks>
    ///     Maksymalna długość 500 znaków, opcjonalny
    /// </remarks>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    ///     Wartość przedmiotu
    /// </summary>
    public decimal Value { get; set; }

    /// <summary>
    ///     Opisowy stan przedmiotu
    /// </summary>
    /// <remarks>
    ///     Wartość opcjonalna
    /// </remarks>
    public string? Condition { get; set; }

    /// <summary>
    ///     Postać, która posiada przedmiot
    /// </summary>
    public Character Character { get; set; }

    /// <summary>
    ///     Id postaci, która posiada przedmiot
    /// </summary>
    public long CharacterId { get; set; }
}