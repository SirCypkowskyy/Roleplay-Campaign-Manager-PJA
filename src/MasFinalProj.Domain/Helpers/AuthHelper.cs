using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MasFinalProj.Domain.Models.Users;
using Microsoft.IdentityModel.Tokens;

namespace MasFinalProj.Domain.Helpers;

/// <summary>
/// Klasa pomocnicza do autoryzacji
/// </summary>
public static class AuthHelper
{
    /// <summary>
    /// Zwraca zahashowane hasło w postaci stringa w formacie Base64.
    /// </summary>
    /// <param name="password">
    /// Hasło do zahashowania.
    /// </param>
    /// <returns>
    /// Zahashowane hasło w formacie Base64 oraz sól w formacie Base64.
    /// </returns>
    public static (string hashPasswrdBase64, string saltBase64) GeneratePasswordHash(string password)
    {
        var salt = GenerateSalt();
        var passwordBytes = HashPassword(password, salt);
        return (Convert.ToBase64String(passwordBytes), salt);
    }

    /// <summary>
    /// Sprawdza czy hasło jest poprawne.
    /// </summary>
    /// <param name="password">
    /// Hasło do sprawdzenia.
    /// </param>
    /// <param name="hashedPassword">
    /// Zahashowane hasło z bazy danych w formacie Base64.
    /// </param>
    /// <param name="salt">
    /// Sól z bazy danych w formacie Base64.
    /// </param>
    /// <returns>
    /// True, jeśli hasła są takie same, w przeciwnym wypadku false.
    /// </returns>
    public static bool PasswordsMatch(string password, string hashedPassword, string salt)
    {
        var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
        var passwordBytes = HashPassword(password, salt);
        return hashedPasswordBytes.SequenceEqual(passwordBytes);
    }

    /// <summary>
    /// Hashuje hasło.
    /// </summary>
    /// <param name="password">
    /// Hasło do zahashowania.
    /// </param>
    /// <param name="salt">
    /// Sól do zahashowania.
    /// </param>
    /// <returns>
    /// Zahashowane hasło.
    /// </returns>
    private static byte[] HashPassword(string password, string salt)
    {
        var saltBytes = Convert.FromBase64String(salt);
        using var rfc2898DeriveBytes = new Rfc2898DeriveBytes(
            password,
            saltBytes,
            10000,
            HashAlgorithmName.SHA256);

        return rfc2898DeriveBytes.GetBytes(256 / 8);
    }


    /// <summary>
    /// Generuje sól.
    /// </summary>
    /// <returns>
    /// Sól w formacie Base64.
    /// </returns>
    private static string GenerateSalt()
    {
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(salt);
        return Convert.ToBase64String(salt);
    }

    /// <summary>
    /// Generuje JWT token.
    /// </summary>
    /// <param name="user">
    /// Użytkownik, dla którego generowany jest token.
    /// </param>
    /// <param name="secret">
    /// Sekret JWT.
    /// </param>
    /// <returns></returns>
    public static (string jwtToken, string refreshToken, DateTime expiryDate) GenerateJwtToken(User user, string secret)
    {
        var role = user switch
        {
            not null when user is Moderator => "Moderator",
            not null when user is Admin => "Admin",
            _ => "User"
        };

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role,  role)
        };
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(15),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        
        return (tokenHandler.WriteToken(token), GenerateJwtRefreshToken(), tokenDescriptor.Expires.Value);
    }

    /// <summary>
    /// Generuje JWT refresh token.
    /// </summary>
    /// <returns></returns>
    private static string GenerateJwtRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}