namespace MasFinalProj.Domain.Models.Campaigns.Characters;

/// <summary>
/// Połączona encja przedmiotu i statystyki
/// </summary>
public class StatItem : CharacterAttribute
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
    
    /// <summary>
    /// Wartość statystyki
    /// </summary>
    public decimal Value { get; set; }
}