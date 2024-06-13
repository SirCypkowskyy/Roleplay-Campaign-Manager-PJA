using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.Models.Users;
using Microsoft.IdentityModel.Tokens;

namespace MasFinalProj.Domain.Helpers;

public static class AuthHelper
{
    public static (string hashPasswrdBase64, string saltBase64) GeneratePasswordHash(string password)
    {
        var salt = GenerateSalt();
        var passwordBytes = HashPassword(password, salt);
        return (Convert.ToBase64String(passwordBytes), salt);
    }

    public static bool PasswordsMatch(string password, string hashedPassword, string salt)
    {
        var hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
        var passwordBytes = HashPassword(password, salt);
        return hashedPasswordBytes.SequenceEqual(passwordBytes);
    }

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

    private static string GenerateSalt()
    {
        var salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(salt);
        return Convert.ToBase64String(salt);
    }

    public static (string jwtToken, string refreshToken, DateTime expiryDate) GenerateJwtToken(User user, ConfigurationOptions configurationOptions)
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
            new("uid", user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString())
        };
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationOptions.JwtSecret));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityKey = new JwtSecurityToken(
            issuer: configurationOptions.JwtIssuer,
            audience: configurationOptions.JwtIssuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: signingCredentials
            );
        
        return (new JwtSecurityTokenHandler().WriteToken(jwtSecurityKey), GenerateJwtRefreshToken(), jwtSecurityKey.ValidTo);
    }

    private static string GenerateJwtRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
            rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber).Replace('+', 'a').Replace('/', 'B').Replace('=', 'c').Replace(" ", "d");
    }
}
