using MasFinalProj.Domain.Models.Campaigns.Characters;

namespace MasFinalProj.Domain.DTOs.Characters.Output;

/// <summary>
/// Odpowiedź z postacią.
/// </summary>
public class CharacterResponseDTO
{
    /// <summary>
    /// Id postaci.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Nazwa postaci.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Id obrazka postaci.
    /// </summary>
    public string ImageId { get; set; }

    public CharacterResponseDTO()
    {
    }
    
    public CharacterResponseDTO(long id, string name)
    {
        Id = id;
        Name = name;
    }
    
    public CharacterResponseDTO(Character character)
    {
        Id = character.Id;
        Name = character.Name;
        ImageId = character.CharacterImageId.ToString() ?? string.Empty;
    }
}