using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;

namespace MasFinalProj.Domain.Models.Campaigns.Characters;

public abstract class CharacterAttribute : BaseEntity<long>
{
    /// <summary>
    /// Nazwa statystyki
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Opis statystyki
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }
    
    /// <summary>
    /// Postać, która posiada atrybut
    /// </summary>
    public Character Character { get; set; }

    /// <summary>
    /// Id postaci, która posiada atrybu
    /// </summary>
    public long CharacterId { get; set; }
    
    /// <summary>
    /// Notatki dotyczące atrybutu
    /// </summary>
    public virtual ICollection<Note> Notes { get; set; } = new List<Note>();
}