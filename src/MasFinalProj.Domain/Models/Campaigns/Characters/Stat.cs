using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;

namespace MasFinalProj.Domain.Models.Campaigns.Characters;

/// <summary>
/// Statystyki kampanii
/// </summary>
public class Stat : CharacterAttribute
{
    /// <summary>
    /// Wartość statystyki
    /// </summary>
    public decimal Value { get; set; }
}