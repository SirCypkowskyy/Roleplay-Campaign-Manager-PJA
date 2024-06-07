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
    [ConfigurationKeyName("Jwt:Key")]
    public string JwtSecret { get; set; }
    
    /// <summary>
    /// Wydawca JWT.
    /// </summary>
    [Required]
    [ConfigurationKeyName("Jwt:Issuer")]
    public string JwtIssuer { get; set; }
}