namespace MasFinalProj.Domain.Repositories;

/// <summary>
/// Interfejs repozytorium użytkowników.
/// </summary>
public interface IUserRepository
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
    /// <param name="userId">
    /// Id użytkownika.
    /// </param>
    /// <returns>
    /// JWT token, JWT refresh token, data wygaśnięcia tokenu oraz nazwa użytkownika.
    /// </returns>
    Task<(string jwtToken, string jwtRefreshToken, DateTime expiryDate, string username)> RefreshTokenAsync(Guid userId, string refreshToken);
}