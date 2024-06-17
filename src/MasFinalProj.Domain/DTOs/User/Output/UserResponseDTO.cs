using MasFinalProj.Domain.Models.Users;

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
        return entity switch
        {
            Admin admin => new UserResponseDTO
            {
                Id = admin.Id, Username = admin.Username, Email = admin.Email
            },
            Moderator moderator => new UserResponseDTO
            {
                Id = moderator.Id, Username = moderator.Username, Email = moderator.Email
            },
            _ => new UserResponseDTO { Id = entity.Id, Username = entity.Username, Email = entity.Email }
        };
    }
}