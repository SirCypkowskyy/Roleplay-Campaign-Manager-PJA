namespace MasFinalProj.Domain.Models.Users;

/// <summary>
/// Klasa reprezentująca administratora
/// </summary>
public class Admin : Moderator
{
    /// <summary>
    /// Czy użytkownik jest super użytkownikiem (może usuwać innych adminów)
    /// </summary>
    public bool IsSuperUser { get; set; } = false;
}