using MasFinalProj.Domain.Models.Campaigns.Characters;

namespace MasFinalProj.Domain.Models.Campaigns.Users;

/// <summary>
/// Klasa użytkownika kampanii
/// </summary>
public class CampaignUserPlayer : CampaignUser
{
    /// <summary>
    /// Ilość postaci kontrolowanych przez gracza
    /// </summary>
    public int CharactersCount => ControlledCharacters.Count;

    /// <summary>
    /// Postacie kontrolowane przez gracza
    /// </summary>
    public virtual ICollection<Character> ControlledCharacters { get; set; } = new List<Character>();
}