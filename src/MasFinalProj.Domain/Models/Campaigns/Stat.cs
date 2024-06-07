using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Models.Campaigns.Characters;

namespace MasFinalProj.Domain.Models.Campaigns;

/// <summary>
/// Statystyki kampanii
/// </summary>
public class Stat : BaseEntity<long>
{
    /// <summary>
    /// Nazwa statystyki
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Opis statystyki
    /// </summary>
    [MaxLength(500)]
    public string Description { get; set; }

    /// <summary>
    /// Opisowa wartość statystyki
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Postać, do której przypisana jest statystyka
    /// </summary>
    public Character Character { get; set; }

    /// <summary>
    /// Id postaci, do której przypisana jest statystyka
    /// </summary>
    public long CharacterId { get; set; }
}