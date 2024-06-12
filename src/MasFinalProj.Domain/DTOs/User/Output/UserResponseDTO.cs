namespace MasFinalProj.Domain.DTOs.User.Output;

/// <summary>
/// DTO odpowiedzi z danymi użytkownika
/// </summary>
public class UserResponseDTO
{
    /// <summary>
    /// Id stworzonego użytkownika
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Nazwa użytkownika
    /// </summary>
    public required string Username { get; set; }

    /// <summary>
    /// Email użytkownika
    /// </summary>
    public required string Email { get; set; }

    /// <summary>
    /// Konwertuje encję użytkownika na DTO odpowiedzi
    /// </summary>
    /// <param name="entity">
    /// Encja użytkownika
    /// </param>
    /// <returns>
    /// DTO odpowiedzi z danymi użytkownika
    /// </returns>
    public static UserResponseDTO FromUser(Domain.Models.Users.User entity)
    {
        return new UserResponseDTO
        {
            Id = entity.Id,
            Username = entity.Username,
            Email = entity.Email,
        };
    }
}