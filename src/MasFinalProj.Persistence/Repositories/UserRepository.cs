using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.Helpers;
using MasFinalProj.Domain.Models.Users.New;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MasFinalProj.Persistence.Repositories;

/// <summary>
/// Implementacja repozytorium użytkowników.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly DatabaseContext _databaseContext;
    private readonly ConfigurationOptions _configurationOptions;
    private readonly ILogger<UserRepository> _logger;

    /// <summary>
    /// Konstruktor repozytorium użytkowników.
    /// </summary>
    /// <param name="databaseContext">
    /// Kontekst bazy danych.
    /// </param>
    /// <param name="logger">
    /// Logger.
    /// </param>
    /// <param name="configurationOptions">
    /// Opcje konfiguracji.
    /// </param>
    public UserRepository(DatabaseContext databaseContext, ILogger<UserRepository> logger, IOptions<ConfigurationOptions> configurationOptions)
    {
        _databaseContext = databaseContext;
        _logger = logger;
        _configurationOptions = configurationOptions.Value;
    }
    
    /// <inheritdoc />
    public async Task<(string jwtToken, string jwtRefreshToken, DateTime expiryDate, string username)> AuthenticateAsync(string email, string password)
    {
        var transaction = await _databaseContext.Database.BeginTransactionAsync();
        
        try
        {
            var user = await _databaseContext.Users
                .FirstOrDefaultAsync(u => u.Email == email);
            
            if (user is null)
            {
                _logger.LogWarning("User with email {Email} not found", email);
                throw new UnauthorizedAccessException("User not found");
            }

            if (!AuthHelper.PasswordsMatch(password, user.PasswordHash, user.PasswordSalt))
            {
                _logger.LogWarning("User with email {Email} provided invalid password", email);
                throw new UnauthorizedAccessException("Invalid password");
            }

            var jwtData = AuthHelper.GenerateJwtToken(user, _configurationOptions.JwtSecret);
            
            _databaseContext.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Value = jwtData.refreshToken,
                ExpiryDateUtc = jwtData.expiryDate.ToUniversalTime()
            });
            
            await _databaseContext.SaveChangesAsync();
            
            await transaction.CommitAsync();

            return (jwtData.jwtToken, jwtData.refreshToken, jwtData.expiryDate, user.Username);
         
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while authenticating user with email {Email}", email);
            await transaction.RollbackAsync();
            throw;
        }
        
    }

    /// <inheritdoc />
    public async Task<(string jwtToken, string jwtRefreshToken, DateTime expiryDate, string username)> RefreshTokenAsync(Guid userId, string refreshToken)
    {
        var transaction = await _databaseContext.Database.BeginTransactionAsync();
        
        try
        {
            var user = await _databaseContext.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
            
            if (user is null)
            {
                _logger.LogWarning("User with id {UserId} not found", userId);
                throw new UnauthorizedAccessException("User not found");
            }

            var dbRefreshToken = await _databaseContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == userId && rt.Value == refreshToken);
            
            if (dbRefreshToken is null)
            {
                _logger.LogWarning("User with id {UserId} provided invalid refresh token", userId);
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            if (dbRefreshToken.ExpiryDateUtc < DateTime.UtcNow)
            {
                _logger.LogWarning("User with id {UserId} provided expired refresh token", userId);
                throw new UnauthorizedAccessException("Expired refresh token");
            }

            var jwtData = AuthHelper.GenerateJwtToken(user, _configurationOptions.JwtSecret);
            
            _databaseContext.RefreshTokens.Remove(dbRefreshToken);
            _databaseContext.RefreshTokens.Add(new RefreshToken
            {
                UserId = user.Id,
                Value = jwtData.refreshToken,
                ExpiryDateUtc = jwtData.expiryDate.ToUniversalTime()
            });
            
            await _databaseContext.SaveChangesAsync();
            
            await transaction.CommitAsync();

            return (jwtData.jwtToken, jwtData.refreshToken, jwtData.expiryDate, user.Username);
         
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while refreshing token for user with id {UserId}", userId);
            await transaction.RollbackAsync();
            throw;
        }
    }
}