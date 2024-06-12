namespace MasFinalProj.Domain.DTOs.User.Output;

/// <summary>
/// Klasa reprezentująca dane odpowiedzi z JWT
/// </summary>
public class UserJwtResponseData
{
    /// <summary>
    /// JWT token
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    /// Refresh token
    /// </summary>
    public string RefreshToken { get; set; }
    
    /// <summary>
    /// Data wygaśnięcia tokenu
    /// </summary>
    public DateTime Expires { get; set; }
    
    /// <summary>
    /// Nazwa użytkownika
    /// </summary>
    public string Username { get; set; }
}