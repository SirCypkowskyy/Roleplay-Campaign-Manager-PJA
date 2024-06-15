namespace MasFinalProj.Persistence.Exceptions;

/// <summary>
/// Wyjątek rzucany, gdy konto użytkownika nie istnieje.
/// </summary>
public class NoAccountException : Exception
{
    /// <summary>
    /// Email użytkownika.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Nazwa użytkownika Discord.
    /// </summary>
    public string DiscordUsername { get; set; }
    
    public NoAccountException(string email) : base($"User with email {email} not found")
    {
        Email = email;
    }
    
    public NoAccountException(string email, string discordUsername) : base($"User with email {email} and Discord username {discordUsername} not found")
    {
        Email = email;
        DiscordUsername = discordUsername;
    }
    
    public NoAccountException(string email, string discordUsername, string message) : base(message)
    {
        Email = email;
        DiscordUsername = discordUsername;
    }
    
    public NoAccountException(string email, string discordUsername, string message, Exception innerException) : base(message, innerException)
    {
        Email = email;
        DiscordUsername = discordUsername;
    }
}