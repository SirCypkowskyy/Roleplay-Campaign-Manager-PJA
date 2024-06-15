using System.Security.Claims;
using Asp.Versioning;
using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.DTOs.User.Input;
using MasFinalProj.Domain.DTOs.User.Output;
using MasFinalProj.Domain.Models.Users;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace MasFinalProj.API.Controllers;

/// <summary>
/// Kontroler użytkownika
/// </summary>
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class UserController : ControllerBase
{
    private readonly ConfigurationOptions _configurationOptions;
    private readonly IUserRepository _userRepository;
    private readonly IDiscordAuthRepository _discordAuthRepository;
    private readonly ILogger<UserController> _logger;
    private readonly HttpClient _httpClient;
    
    private static readonly Dictionary<string, UserJwtResponseData> _tempCodeTokens = new();
    
    /// <summary>
    /// Konstruktor kontrolera użytkownika
    /// </summary>
    /// <param name="configurationOptions">
    /// Opcje konfiguracji
    /// </param>
    /// <param name="userRepository">
    /// Repozytorium użytkowników
    /// </param>
    /// <param name="logger">
    /// Logger
    /// </param>
    /// <param name="discordAuthRepository">
    /// Repozytorium autoryzacji Discorda
    /// </param>
    /// <param name="httpClientFactory">
    /// Fabryka klientów HTTP
    /// </param>
    public UserController(IOptions<ConfigurationOptions> configurationOptions, 
        IUserRepository userRepository, ILogger<UserController> logger, 
        IDiscordAuthRepository discordAuthRepository, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _configurationOptions = configurationOptions.Value;
        _userRepository = userRepository;
        _discordAuthRepository = discordAuthRepository;
        _httpClient = httpClientFactory.CreateClient("discordClient");
    }


    /// <summary>
    /// Metoda do autoryzacji użytkownika
    /// </summary>
    /// <param name="loginData">
    /// Dane logowania
    /// </param>
    /// <returns></returns>
    [HttpPost("auth")]
    [ApiVersion("1.0")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserJwtResponseData))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AuthenticateAsync([FromBody] UserLoginInputDataDTO loginData)
    {
        _logger.LogInformation("User with email {Email} is trying to authenticate", loginData.Email);
        try
        {
            var dbResponse = await _userRepository.AuthenticateAsync(loginData.Email, loginData.Password);
            return Ok(new UserJwtResponseData
            {
                Token = dbResponse.jwtToken,
                RefreshToken = dbResponse.jwtRefreshToken,
                Expires = dbResponse.expiryDate,
                Username = dbResponse.username
            });
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogWarning(e, "Error while authenticating user with email {Email}", loginData.Email);
            return Unauthorized("Niepoprawny email i/lub hasło!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while authenticating user with email {Email}", loginData.Email);
            return BadRequest("Wystąpił nieznany błąd podczas autoryzacji!");
        }
    }

    public class RefreshTokenObj
    {
        public string RefreshToken { get; set; }
    }
    
    /// <summary>
    /// Metoda do odświeżania tokena JWT
    /// </summary>
    /// <param name="refreshToken">
    /// Token odświeżający
    /// </param>
    /// <param name="email">
    /// Email użytkownika
    /// </param>
    /// <returns></returns>
    [HttpPost("auth/refresh")]
    public async Task<IActionResult> RefreshTokenAsync([FromBody] RefreshTokenObj refreshToken)
    {
        try
        {
            var dbResponse = await _userRepository.RefreshTokenAsync(refreshToken.RefreshToken);
            return Ok(new UserJwtResponseData
            {
                Token = dbResponse.jwtToken,
                RefreshToken = dbResponse.jwtRefreshToken,
                Expires = dbResponse.expiryDate,
                Username = dbResponse.username
            });
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogWarning(e, "Error while refreshing token");
            return Unauthorized($"Wystąpił błąd podczas odświeżania tokenu: {e.Message}");
        }
    }


    /// <summary>
    /// Zwraca informacje o zalogowanym użytkowniku
    /// </summary>
    /// <returns>
    /// Informacje o zalogowanym użytkowniku
    /// </returns>
    [HttpGet("auth/self")]
    [Authorize]
    public async Task<IActionResult> GetSelfInfoAsync()
    {
        var user = User;
        var claims = user.Claims;
        
        _logger.LogInformation("User {Username} requested self info", user.FindFirstValue(ClaimTypes.Name));
        _logger.LogDebug("User {Username} requested self info with claims: {Claims}", user.FindFirstValue(ClaimTypes.Name), claims);
        
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User not found");
        
        return Ok(new UserJwtInfoResponseDTO()
        {
            Username = user.FindFirstValue(ClaimTypes.Name),
            Email = user.FindFirstValue(ClaimTypes.Email),
            Role = user.FindFirstValue(ClaimTypes.Role)
        });
    }
    
    /// <summary>
    /// Metoda do autoryzacji użytkownika przez Discorda
    /// </summary>
    /// <returns></returns>
    [HttpGet("auth/discord")]
    public async Task<IActionResult> AuthenticateDiscordAsync()
    {
        var redirectUrl = "https://discord.com/api/oauth2/authorize?" +
                          $"client_id={_configurationOptions.DiscordClientId}&" +
                          $"redirect_uri={"http://localhost:5128/api/v1/user/auth/discord/callback"}&" +
                          $"response_type={_configurationOptions.DiscordResponseType}&" +
                          $"scope={_configurationOptions.DiscordScope}";
        
        return Redirect(redirectUrl);
    }
    
    /// <summary>
    /// Callback autoryzacji użytkownika przez Discorda
    /// </summary>
    /// <remarks>
    /// NIE UŻYWAJ JAKO UŻYTKOWNIK, JEST TO TYLKO DLA DISCORDA (dlatego jest oznaczone jako SwaggerIgnore)
    /// </remarks>
    /// <returns></returns>
    [HttpGet("auth/discord/callback")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerIgnore]
    public async Task<IActionResult> AuthenticateDiscordCallbackAsync()
    {
        try
        {
            var queryParameters = Request.Query;
        
            if (queryParameters.ContainsKey("error"))
            {
                _logger.LogWarning("Discord API returned error: {Error}", queryParameters["error"]);
                return BadRequest("Discord API returned error");
            }
        
            if (!queryParameters.ContainsKey("code"))
            {
                _logger.LogWarning("Discord API did not return code");
                return BadRequest("Discord API did not return code");
            }
        
            var code = queryParameters["code"];

            if (string.IsNullOrWhiteSpace(code))
            {
                _logger.LogWarning("Discord API returned empty code");
                return BadRequest("Discord API returned empty code");
            }

            var authData = await _discordAuthRepository.AuthenticateDiscordAsync(code);
        
            _logger.LogInformation("User {Username} authenticated via Discord", authData.username);

            var tempCode = Guid.NewGuid().ToString().Replace("-", "")[..8];
        
            while (_tempCodeTokens.ContainsKey(tempCode))
                tempCode = Guid.NewGuid().ToString().Replace("-", "")[..8];

            _tempCodeTokens[tempCode] = new UserJwtResponseData()
            {
                Token = authData.jwtToken,
                RefreshToken = authData.jwtRefreshToken,
                Expires = authData.expiryDate,
                Username = authData.username
            };
            // TODO: Dodaj usuwanie po czasie (jeśli jest expired)
        
            _logger.LogInformation("Generated temp code {TempCode} for user {Username}", tempCode, authData.username);
            _logger.LogInformation("Existing temp codes: {Codes}", _tempCodeTokens.Keys);
            var redirectUrl = $"http://localhost:5129/dashboard?code={tempCode}";
        
            return Redirect(redirectUrl);
        }
        catch(NoAccountException e)
        {
            _logger.LogWarning("User tried to authenticate via Discord while account not found");
            _logger.LogInformation("Redirecting to registration page");
            return Redirect($"http://localhost:5129/register?email={e.Email}&username={e.DiscordUsername}");
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogWarning(e, "Error while authenticating user via Discord");
            return BadRequest("Error while authenticating user via Discord");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while authenticating user via Discord");
            return BadRequest("Error while authenticating user via Discord");
        }
    }

    /// <summary>
    /// Metoda do pobierania tymczasowych danych JWT z kodu
    /// </summary>
    /// <param name="code">
    /// Kod do pobrania danych
    /// </param>
    /// <returns></returns>
    [HttpGet("auth/discord/retrieve")]
    [AllowAnonymous]
    public async Task<IActionResult> RetrieveTempJwtDataAsync([FromQuery] string code)
    {
        _logger.LogInformation("User requested temp JWT data with code {Code}", code);
        _logger.LogInformation("Existing temp codes: {Codes}", _tempCodeTokens.Keys);
        
        if (_tempCodeTokens.Remove(code, out var data)) return Ok(data);
        
        _logger.LogWarning("Code {Code} not found", code);
        return BadRequest("Code not found");
    }

    /// <summary>
    /// Metoda do tworzenia konta
    /// </summary>
    /// <param name="createUserInputDto">
    /// Dane do utworzenia konta
    /// </param>
    /// <returns>
    /// Odpowiedź ze stworzonym kontem
    /// </returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserResponseDTO))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost("register")]
    public async Task<IActionResult> CreateAccountAsync([FromBody] CreateUserInputDTO createUserInputDto)
    {
        try
        {
            var dbResponse = await _userRepository.CreateUserAsync(
                createUserInputDto.Email,
                createUserInputDto.Username,
                password: createUserInputDto.Password
                );
            
            return Ok(UserResponseDTO.FromUser(dbResponse));
        }
        catch (ArgumentException e)
        {
            _logger.LogWarning(e, "Error while creating user with email {Email}", createUserInputDto.Email);
            return BadRequest("Podany adres email, nazwa użytkownika lub hasło nie są poprawne w zapisie!");
        }
        catch (UnauthorizedAccessException e)
        {
            _logger.LogWarning(e, "Error while creating user with email {Email}", createUserInputDto.Email);
            return BadRequest("Podany adres email lub nazwa użytkownika są już zajęte, lub email jest na czarnej liście!");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating user with email {Email}", createUserInputDto.Email);
            return BadRequest("Wystąpił nieznany błąd podczas tworzenia konta!");
        }
    }

    /// <summary>
    /// Metoda do pobierania danych do dashboardu użytkownika
    /// </summary>
    /// <returns></returns>
    [Authorize]
    [HttpGet("dashboard")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDashboardDataDTO))]
    public async Task<IActionResult> GetUserDashboardData()
    {
        var user = User;
        var claims = user.Claims;
        
        _logger.LogInformation("User {Username} requested dashboard data", user.FindFirstValue(ClaimTypes.Name));
        _logger.LogDebug("User {Username} requested dashboard data with claims: {Claims}", user.FindFirstValue(ClaimTypes.Name), claims);
        
        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new UnauthorizedAccessException("User not found");
        
        var userDashboardData = await _userRepository.GetUserDashboardDataAsync(userId);
        
        return Ok(userDashboardData);
    }
}