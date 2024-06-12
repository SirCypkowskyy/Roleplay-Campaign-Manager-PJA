namespace MasFinalProj.Domain.DTOs.User.Output;

/// <summary>
/// DTO z danymi użytkownika w tokenie JWT
/// </summary>
public class UserJwtInfoResponseDTO
{
    /// <summary>
    /// Nazwa użytkownika
    /// </summary>
    public string Username { get; set; }

    /// <summary>
    /// Email użytkownika
    /// </summary>
    public string Email { get; set; }
    
    /// <summary>
    /// Rola użytkownika
    /// </summary>
    public string Role { get; set; }
}