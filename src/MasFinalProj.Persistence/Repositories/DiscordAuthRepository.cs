using System.Text.Json;
using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.Helpers;
using MasFinalProj.Domain.Models.NoDbModels;
using MasFinalProj.Domain.Models.Users;
using MasFinalProj.Domain.Models.Users.New;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Data;
using Microsoft.Extensions.Options;

namespace MasFinalProj.Persistence.Repositories;

/// <summary>
/// Implementacja repozytorium autoryzacji Discorda.
/// </summary>
public class DiscordAuthRepository : IDiscordAuthRepository
{
    private readonly HttpClient _httpClient;
    private static readonly string _httpClientName = "discordClient";
    private readonly ConfigurationOptions _configurationOptions;
    private readonly IUserRepository _userRepository;
    private readonly DatabaseContext _databaseContext;

    public DiscordAuthRepository(IHttpClientFactory httpClientFactory,
        IOptions<ConfigurationOptions> configurationOptions,
        IUserRepository userRepository,
        DatabaseContext databaseContext)
    {
        _configurationOptions = configurationOptions.Value;
        _httpClient = httpClientFactory.CreateClient(_httpClientName);
        _userRepository = userRepository;
        _databaseContext = databaseContext;
    }

    /// <inheritdoc />
    public async Task<(string jwtToken, string jwtRefreshToken, DateTime expiryDate, string username)>
        AuthenticateDiscordAsync(string code)
    {
        var tokenResponse = await _httpClient.PostAsync("https://discordapp.com/api/oauth2/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["client_id"] = _configurationOptions.DiscordClientId,
                ["client_secret"] = _configurationOptions.DiscordClientSecret,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = _configurationOptions.DiscordRedirectUri
            }));

        tokenResponse.EnsureSuccessStatusCode();

        var tokenResponseContent = await tokenResponse.Content.ReadAsStringAsync();

        var token = JsonSerializer.Deserialize<DiscordTokenResponse>(tokenResponseContent);

        var request = new HttpRequestMessage(HttpMethod.Get, "https://discordapp.com/api/users/@me");

        request.Headers.Add("Authorization", $"{token.TokenType} {token.AccessToken}");
        // request.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

        var userResponse = await _httpClient.SendAsync(request);
        var userResponseContent = await userResponse.Content.ReadAsStringAsync();

        userResponse.EnsureSuccessStatusCode();



        var user = JsonSerializer.Deserialize<DiscordUser>(userResponseContent);

        if (user is null)
            throw new UnauthorizedAccessException("User nie mógł zostać pobrany z Discorda");

        var dbUser = await _userRepository.FirstOrDefaultAsync(x => x.Email == user.Email);

        if (dbUser is null)
            throw new UnauthorizedAccessException("User nie istnieje w bazie danych");

        dbUser.DiscordUsername = user.Username;
        dbUser.DiscordId = long.Parse(user.Id);
        await _userRepository.UpdateAsync(dbUser);

        var jwtData = AuthHelper.GenerateJwtToken(dbUser, _configurationOptions);

        _databaseContext.RefreshTokens.Add(new RefreshToken
        {
            UserId = dbUser.Id,
            Value = jwtData.refreshToken,
            ExpiryDateUtc = jwtData.expiryDate.ToUniversalTime()
        });
            
        await _databaseContext.SaveChangesAsync();
        
        return (jwtData.jwtToken, jwtData.refreshToken, jwtData.expiryDate, dbUser.Username);
    }
}