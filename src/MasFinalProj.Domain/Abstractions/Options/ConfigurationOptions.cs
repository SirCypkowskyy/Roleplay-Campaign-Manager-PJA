using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace MasFinalProj.Domain.Abstractions.Options;

/// <summary>
/// Klasa reprezentująca opcje aplikacji (w ramach Options Pattern).
/// </summary>
public class ConfigurationOptions
{
    /// <summary>
    /// Connection string do bazy danych.
    /// </summary>
    [Required]
    [ConfigurationKeyName("ConnectionStrings:DefaultConnection")]
    public string DbConnectionString { get; set; }
    
    /// <summary>
    /// Adresy, z których można korzystać z API.
    /// </summary>
    [Required]
    [ConfigurationKeyName("AllowedOrigins")]
    public string[] AllowedOrigins { get; set; }
    
    /// <summary>
    /// Sekret JWT.
    /// </summary>
    [Required]
    [ConfigurationKeyName("Auth:Jwt:Key")]
    public string JwtSecret { get; set; }
    
    /// <summary>
    /// Wydawca JWT.
    /// </summary>
    [Required]
    [ConfigurationKeyName("Auth:Jwt:Issuer")]
    public string JwtIssuer { get; set; }

    public string? Type { get; set; }

    /// <summary>
    /// Identyfikator klienta Discord.
    /// </summary>
    /// <remarks>
    /// https://www.yogihosting.com/discord-api-asp-net/
    /// </remarks>
    [ConfigurationKeyName("Auth:Discord:ClientId")]
    public string? DiscordClientId { get; set; }
    
    /// <summary>
    /// Sekret klienta Discord.
    /// </summary>
    /// <remarks>
    /// https://www.yogihosting.com/discord-api-asp-net/
    /// </remarks>
    [ConfigurationKeyName("Auth:Discord:ClientSecret")]
    public string? DiscordClientSecret { get; set; }

    /// <summary>
    /// Typ grantu Discord.
    /// </summary>
    /// <remarks>
    /// https://www.yogihosting.com/discord-api-asp-net/
    /// </remarks>
    [ConfigurationKeyName("Auth:Discord:GrantType")]
    public string? DiscordGrantType { get; set; }
    
    /// <summary>
    /// Kod Discord.
    /// </summary>
    /// <remarks>
    /// https://www.yogihosting.com/discord-api-asp-net/
    /// </remarks>
    [ConfigurationKeyName("Auth:Discord:ResponseType")]
    public string? DiscordResponseType { get; set; }

    /// <summary>
    /// Zakres informacji Discord.
    /// </summary>
    /// <remarks>
    /// https://www.yogihosting.com/discord-api-asp-net/
    /// </remarks>
    [ConfigurationKeyName("Auth:Discord:Scope")]
    public string? DiscordScope { get; set; }

    /// <summary>
    /// Przekierowanie Discord.
    /// </summary>
    /// <remarks>
    /// https://www.yogihosting.com/discord-api-asp-net/
    /// </remarks>
    [ConfigurationKeyName("Auth:Discord:RedirectUri")]
    public string? DiscordRedirectUri { get; set; }
}