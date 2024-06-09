using System.Text.Json.Serialization;

namespace MasFinalProj.Domain.Models.NoDbModels;

/// <summary>
/// Model odpowiedzi z autoryzacji Discorda.
/// </summary>
public class DiscordTokenResponse
{
    /// <summary>
    /// Rodzaj tokenu.
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }

    /// <summary>
    /// Wartość tokenu.
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    
    /// <summary>
    /// Czas wygaśnięcia tokenu.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; }
    
    /// <summary>
    /// Przyznane uprawnienia.
    /// </summary>
    [JsonPropertyName("scope")]
    public string Scope { get; set; }
}