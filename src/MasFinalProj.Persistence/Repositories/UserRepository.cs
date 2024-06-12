using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.DTOs.User.Output;
using MasFinalProj.Domain.Helpers;
using MasFinalProj.Domain.Models.Campaigns.Users;
using MasFinalProj.Domain.Models.Users;
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
public class UserRepository : GenericRepository<Guid, User>, IUserRepository
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
    public UserRepository(DatabaseContext databaseContext, ILogger<UserRepository> logger, IOptions<ConfigurationOptions> configurationOptions) : base(databaseContext, logger)
    {
        _databaseContext = databaseContext;
        _logger = logger;
        ArgumentNullException.ThrowIfNull(configurationOptions);
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

            var jwtData = AuthHelper.GenerateJwtToken(user, _configurationOptions);
            
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
    public async Task<(string jwtToken, string jwtRefreshToken, DateTime expiryDate, string username)> RefreshTokenAsync(string username, string refreshToken)
    {
        var transaction = await _databaseContext.Database.BeginTransactionAsync();
        
        try
        {
            var user = await _databaseContext.Users
                .FirstOrDefaultAsync(u => u.Username == username);
            
            if (user is null)
            {
                _logger.LogWarning("User with username {Username} not found", username);
                throw new UnauthorizedAccessException("User not found");
            }

            var dbRefreshToken = await _databaseContext.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.UserId == user.Id && rt.Value == refreshToken);
            
            if (dbRefreshToken is null)
            {
                _logger.LogWarning("User with id {UserId} provided invalid refresh token", user.Id);
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            if (dbRefreshToken.ExpiryDateUtc < DateTime.UtcNow)
            {
                _logger.LogWarning("User with id {UserId} provided expired refresh token", user.Id);
                throw new UnauthorizedAccessException("Expired refresh token");
            }

            var jwtData = AuthHelper.GenerateJwtToken(user, _configurationOptions);
            
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
            _logger.LogError(e, "Error while refreshing token for user with username {Username}", username);
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<User> CreateUserAsync(string email, string username, string password)
    {
        var transaction = await _databaseContext.Database.BeginTransactionAsync();
        
        try
        {
            await ValidateEmailAndUsernameInDbAsync(email, username);

            var user = new User
            {
                Email = email,
                Username = username
            };
            
            var authData = AuthHelper.GeneratePasswordHash(password);
            user.PasswordHash = authData.hashPasswrdBase64;
            user.PasswordSalt = authData.saltBase64;
            
            await _databaseContext.Users.AddAsync(user);
            await _databaseContext.SaveChangesAsync();
            
            await transaction.CommitAsync();

            return user;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while creating user with email {Email}", email);
            await transaction.RollbackAsync();
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<UserDashboardDataDTO> GetUserDashboardDataAsync(string userId)
    {
        var userIdAsGuid = Guid.Parse(userId);
        
        var messagesSentTotal = await _databaseContext.Messages
            .Include(m => m.Author)
            .CountAsync(m => m.Author.UserId == userIdAsGuid);
        
        var messagesSentInLast30Days = await _databaseContext.Messages
            .Include(m => m.Author)
            .CountAsync(m => m.Author.UserId == userIdAsGuid && m.CreatedAtUtc > DateTime.UtcNow.AddDays(-30));
        
        var activeCampaigns = await _databaseContext.Campaigns
            .Include(c => c.CampaignGameMaster).ThenInclude(campaignUserGameMaster => campaignUserGameMaster.User)
            .Include(c => c.CampaignPlayers)
            .Where(c => c.CampaignPlayers.Any(cp => cp.UserId == userIdAsGuid) || c.CampaignGameMaster.Any(cgm => cgm.UserId == userIdAsGuid))
            .Select(c => new { c.Id, c.Name, c.CampaignGameMaster, c.CampaignPlayers, c.Messages.Max(c => c.CreatedAtUtc).Date })
            .ToListAsync();
        
        var charactersCreatedTotal = await _databaseContext.Characters
            .Include(c => c.PlayerOwner)
            .Include(c => c.Campaign).ThenInclude(c => c.CampaignGameMaster)
            .CountAsync(c => (c.PlayerOwner != null && c.PlayerOwner.UserId == userIdAsGuid) || c.Campaign.CampaignGameMaster.Any(cp => cp.UserId == userIdAsGuid));

        return new UserDashboardDataDTO()
        {
            NumberOfActiveCampaignsTotal = activeCampaigns.Count,
            NumberOfActiveCampaignsInLast30Days = activeCampaigns.Count(c => c.Date > DateTime.UtcNow.AddDays(-30)),
            MessagesSentTotal = messagesSentTotal,
            MessagesSentInLast30Days = messagesSentInLast30Days,
            CreatedCharactersTotal = charactersCreatedTotal,
            ActiveCampaigns = activeCampaigns.Select(c => new DashboardCampaignResponseDTO
            {
                CampaignGuid = c.Id.ToString(),
                CampaignHost = c.CampaignGameMaster.First().User.Username,
                CampaignHostGuid = c.CampaignGameMaster.First().UserId.ToString(),
                CampaignName = c.Name,
                LastMessageDate = DateOnly.FromDateTime(c.Date)
            }).ToArray()
        };
    }

    /// <summary>
    /// Waliduje email i nazwę użytkownika w bazie danych.
    /// </summary>
    /// <param name="email"></param>
    /// <param name="username"></param>
    /// <exception cref="UnauthorizedAccessException"></exception>
    private async Task ValidateEmailAndUsernameInDbAsync(string email, string username)
    {
        if(await _databaseContext.BlacklistedEmails
               .AnyAsync(be => be.Email == email))
            throw new UnauthorizedAccessException("Email is blacklisted");
            
        if(await _databaseContext.Users
               .AnyAsync(u => u.Email == email))
            throw new UnauthorizedAccessException("Email is already in use");
            
        if(await _databaseContext.Users
               .AnyAsync(u => u.Username == username))
            throw new UnauthorizedAccessException("Username is already in use");
    }
}