using System.ComponentModel.DataAnnotations;

namespace MasFinalProj.API.DTOs.User.Input;

/// <summary>
/// DTO z danymi wejściowymi do logowania użytkownika
/// </summary>
public class UserLoginInputDataDTO
{
    /// <summary>
    /// Email użytkownika
    /// </summary>
    [MaxLength(32)]
    public string Email { get; set; }
    
    /// <summary>
    /// Hasło użytkownika
    /// </summary>
    [MinLength(6)]
    public string Password { get; set; }
}