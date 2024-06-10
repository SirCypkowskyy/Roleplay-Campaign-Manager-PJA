using System.Text.Json.Serialization;

namespace MasFinalProj.Domain.Models.NoDbModels;


/// <summary>
/// Model użytkownika Discorda.
/// </summary>
public class DiscordUser
{
    /// <summary>
    /// Id użytkownika.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    /// <summary>
    /// Nazwa użytkownika.
    /// </summary>
    [JsonPropertyName("username")]
    public string Username { get; set; }

    // [JsonPropertyName("avatar")]
    // public string? Avatar { get; set; }
    //
    // [JsonPropertyName("discriminator")]
    // public string? Discriminator { get; set; }
    //
    // [JsonPropertyName("public_flags")]
    // public string? PublicFlags { get; set; }
    //
    // [JsonPropertyName("flags")]
    // public string? Flags { get; set; }
    //
    // [JsonPropertyName("banner")]
    // public string? Banner { get; set; }
    //
    // [JsonPropertyName("accent_color")]
    // public string? AccentColor { get; set; }
    //
    // [JsonPropertyName("global_name")]
    // public string? GlobalName { get; set; }
    //
    // [JsonPropertyName("avatar_decoration_data")]
    // public string? AvatarDecorationData { get; set; }
    //
    // [JsonPropertyName("banner_color")]
    // public string? BannerColor { get; set; }
    //
    // [JsonPropertyName("clan")]
    // public string? Clan { get; set; }
    //
    // [JsonPropertyName("mfa_enabled")]
    // public bool? MfaEnabled { get; set; }
    //
    // [JsonPropertyName("locale")]
    // public string? Locale { get; set; }
    //
    // [JsonPropertyName("premium_type")]
    // public string? PremiumType { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    // [JsonPropertyName("verified")]
    // public bool? Verified { get; set; }
}