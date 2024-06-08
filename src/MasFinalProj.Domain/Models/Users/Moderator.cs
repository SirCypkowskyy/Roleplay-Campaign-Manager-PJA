namespace MasFinalProj.Domain.Models.Users;

/// <summary>
/// Encja moderatora aplikacji
/// </summary>
public class Moderator : User
{
    /// <summary>
    /// Data, od której pełni obowiązki.
    /// </summary>
    public DateTime StaffSinceUtc { get; set; }
}