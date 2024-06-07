using MasFinalProj.API.DTOs.User.Input;
using MasFinalProj.API.DTOs.User.Output;
using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MasFinalProj.API.Controllers;

/// <summary>
/// Kontroler użytkownika
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ConfigurationOptions _configurationOptions;
    private readonly IUserRepository _userRepository;
    
    /// <summary>
    /// Konstruktor kontrolera użytkownika
    /// </summary>
    /// <param name="configurationOptions">
    /// Opcje konfiguracji
    /// </param>
    /// <param name="userRepository">
    /// Repozytorium użytkowników
    /// </param>
    public UserController(ConfigurationOptions configurationOptions, IUserRepository userRepository)
    {
        _configurationOptions = configurationOptions;
        _userRepository = userRepository;
    }


    /// <summary>
    /// Metoda do autoryzacji użytkownika
    /// </summary>
    /// <param name="loginData">
    /// Dane logowania
    /// </param>
    /// <returns></returns>
    [HttpPost("auth")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserJwtResponseData))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> AuthenticateAsync([FromBody] UserLoginInputDataDTO loginData)
    {
        try
        {
            var dbResponse = await _userRepository.AuthenticateAsync(loginData.Email, loginData.Password);
            return Ok(new UserJwtResponseData
            {
                JwtToken = dbResponse.jwtToken,
                RefreshToken = dbResponse.jwtRefreshToken,
                Expires = dbResponse.expiryDate,
                Username = dbResponse.username
            });
        }
        catch (UnauthorizedAccessException e)
        {
            return Unauthorized(e.Message);
        }
    }
}