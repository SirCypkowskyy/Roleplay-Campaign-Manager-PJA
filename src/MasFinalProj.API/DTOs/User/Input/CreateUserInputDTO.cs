using System.ComponentModel.DataAnnotations;

namespace MasFinalProj.API.DTOs.User.Input;

/// <summary>
/// DTO do tworzenia użytkownika
/// </summary>
public class CreateUserInputDTO
{
    /// <summary>
    /// Nazwa użytkownika
    /// </summary>
    [MaxLength(32)]
    public string Username { get; set; }

    /// <summary>
    /// Adres email użytkownika
    /// </summary>
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Hasło użytkownika
    /// </summary>
    [MinLength(6)]
    [MaxLength(32)]
    public string Password { get; set; }
}