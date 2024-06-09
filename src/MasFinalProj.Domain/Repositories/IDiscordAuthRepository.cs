namespace MasFinalProj.Domain.Repositories;

/// <summary>
/// Interfejs repozytorium autoryzacji Discorda.
/// </summary>
public interface IDiscordAuthRepository
{
    /// <summary>
    /// Autoryzuje użytkownika Discorda
    /// </summary>
    /// <param name="code">
    /// Kod autoryzacji Discorda.
    /// </param>
    /// <returns></returns>
    Task<(string jwtToken, string jwtRefreshToken, DateTime expiryDate, string username)> AuthenticateDiscordAsync(string code);
}