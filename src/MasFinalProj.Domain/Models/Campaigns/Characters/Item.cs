using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;

namespace MasFinalProj.Domain.Models.Campaigns.Characters;

/// <summary>
/// Encja przedmiotu
/// </summary>
/// <remarks>
/// Sortowana po nazwie
/// </remarks>
public class Item : CharacterAttribute
{
    /// <summary>
    /// Wartość przedmiotu
    /// </summary>
    public decimal MoneyValue { get; set; }
    
    /// <summary>
    /// Stan przedmiotu
    /// </summary>
    public string? Condition { get; set; }
    
    /// <summary>
    /// Modyfikator przedmiotu
    /// </summary>
    public string? Modifier { get; set; }
}