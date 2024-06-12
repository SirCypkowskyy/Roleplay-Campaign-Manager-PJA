using MasFinalProj.Domain.DTOs.User.Output;
using MasFinalProj.Domain.Models.Users;

namespace MasFinalProj.Domain.Repositories;

/// <summary>
/// Interfejs repozytorium użytkowników.
/// </summary>
public interface IUserRepository : IGenericRepository<Guid, User>
{
    /// <summary>
    /// Autoryzuje użytkownika.
    /// </summary>
    /// <param name="email">
    /// Adres email użytkownika.
    /// </param>
    /// <param name="password">
    /// Hasło użytkownika.
    /// </param>
    /// <returns>
    /// JWT token, JWT refresh token, data wygaśnięcia tokenu oraz nazwa użytkownika.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Jeśli podany adres email lub hasło nie są poprawne w zapisie.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Jeśli email i/lub hasło są niepoprawne.
    /// </exception>
    Task<(string jwtToken, string jwtRefreshToken, DateTime expiryDate, string username)> AuthenticateAsync(string email, string password);
    
    /// <summary>
    /// Odświeża token.
    /// </summary>
    /// <param name="refreshToken">
    /// Token odświeżający.
    /// </param>
    /// <param name="username">
    /// Nazwa użytkownika.
    /// </param>
    /// <returns>
    /// JWT token, JWT refresh token, data wygaśnięcia tokenu oraz nazwa użytkownika.
    /// </returns>
    Task<(string jwtToken, string jwtRefreshToken, DateTime expiryDate, string username)> RefreshTokenAsync(string username, string refreshToken);

    /// <summary>
    /// Tworzy nowego użytkownika.
    /// </summary>
    /// <param name="email">
    /// Adres email użytkownika.
    /// </param>
    /// <param name="username">
    /// Nazwa użytkownika.
    /// </param>
    /// <param name="password">
    /// Hasło użytkownika.
    /// </param>
    /// <returns>
    /// Utworzony użytkownik.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Jeśli podany adres email, nazwa użytkownika lub hasło nie są poprawne w zapisie.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Jeśli adres email lub nazwa użytkownika są już zajęte, lub jeśli email jest na czarnej liście.
    /// </exception>
    Task<User> CreateUserAsync(string email, string username, string password);

    /// <summary>
    /// Pobiera dane użytkownika do wyświetlenia na prywatnym dashboardzie.
    /// </summary>
    /// <param name="userId">
    /// Id użytkownika.
    /// </param>
    /// <returns>
    /// DTO z danymi do wyświetlenia na prywatnym dashboardzie użytkownika.
    /// </returns>
    Task<UserDashboardDataDTO> GetUserDashboardDataAsync(string userId);
}