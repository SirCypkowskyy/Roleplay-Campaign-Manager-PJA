using MasFinalProj.Domain.Abstractions.Models;

namespace MasFinalProj.Domain.Models.Users.New;

/// <summary>
/// Encja odświeżającego tokenu
/// </summary>
public class RefreshToken : BaseEntity<Guid>
{
    /// <summary>
    /// Id użytkownika
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Użytkownik
    /// </summary>
    public User User { get; set; }
    
    /// <summary>
    /// Wartość tokenu
    /// </summary>
    public string Value { get; set; }
    
    /// <summary>
    /// Data wygaśnięcia tokenu
    /// </summary>
    public DateTime ExpiryDateUtc { get; set; }
}