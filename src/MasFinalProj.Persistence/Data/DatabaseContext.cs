using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Models.Campaigns.Characters;
using MasFinalProj.Domain.Models.Campaigns.Users;
using MasFinalProj.Domain.Models.Common;
using MasFinalProj.Domain.Models.Users;
using MasFinalProj.Domain.Models.Users.New;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MasFinalProj.Persistence.Data;

/// <summary>
/// Kontekst bazy danych
/// </summary>
public class DatabaseContext : DbContext
{
    /// <summary>
    /// Kampanie
    /// </summary>
    public DbSet<Campaign> Campaigns { get; set; }
    
    /// <summary>
    /// Postacie w ramach różnych kampanii
    /// </summary>
    public DbSet<Character> Characters { get; set; }
    
    /// <summary>
    /// Relacje postaci z innymi postaciami
    /// </summary>
    public DbSet<CharacterRelationWith> CharacterRelationsWith { get; set; }
    
    /// <summary>
    /// Uczestnicy kampanii
    /// </summary>
    public DbSet<CampaignUser> CampaignUsers { get; set; }
    
    /// <summary>
    /// Mistrzowie gier w ramach kampanii
    /// </summary>
    public DbSet<CampaignUserGameMaster> CampaignUserGameMasters { get; set; }
    
    /// <summary>
    /// Gracze w ramach kampanii
    /// </summary>
    public DbSet<CampaignUserPlayer> CampaignUserPlayers { get; set; }
    
    /// <summary>
    /// Przedmioty w ramach kampanii
    /// </summary>
    public DbSet<Item> Items { get; set; }
    
    /// <summary>
    /// Wiadomości
    /// </summary>
    public DbSet<Message> Messages { get; set; }
    
    /// <summary>
    /// Notatki
    /// </summary>
    public DbSet<Note> Notes { get; set; }
    
    /// <summary>
    /// Statystyki
    /// </summary>
    public DbSet<Stat> Stats { get; set; }
    
    /// <summary>
    /// Zdjęcia
    /// </summary>
    public DbSet<Image> Images { get; set; }
    
    /// <summary>
    /// Użytkownicy
    /// </summary>
    public DbSet<User> Users { get; set; }
    
    /// <summary>
    /// Moderatorzy
    /// </summary>
    public DbSet<Moderator> Moderators { get; set; }
    
    /// <summary>
    /// Administratorzy
    /// </summary>
    public DbSet<Admin> Admins { get; set; }
    
    /// <summary>
    /// Tokeny odświeżające
    /// </summary>
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
    /// <summary>
    /// Lista zablokowanych adresów email
    /// </summary>
    public DbSet<BlacklistedEmail> BlacklistedEmails { get; set; }
    
    // /// <summary>
    // /// Kolekcja encji <typeparamref name="TEntity"/> z kluczem typu <typeparamref name="TKey"/>
    // /// </summary>
    // /// <typeparam name="TEntity">
    // /// Typ encji
    // /// </typeparam>
    // /// <typeparam name="TKey">
    // /// Typ klucza
    // /// </typeparam>
    // /// <returns></returns>
    // public DbSet<TEntity> Set<TEntity, TKey>() where TEntity : BaseEntity<TKey> where TKey : struct
    //     => base.Set<TEntity>();

    private readonly string _connectionString;
    
    public DatabaseContext(IOptions<ConfigurationOptions> configuration)
    {
        _connectionString = configuration.Value.DbConnectionString;
    }
 

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(DatabaseContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }


    /// <inheritdoc />
    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateAuditableEntities();
        
        CheckValidatableEntities();
        
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    /// <summary>
    /// Sprawdza encje walidowalne (dziedziczące po <see cref="IValidateOnSave"/>)
    /// </summary>
    private void CheckValidatableEntities()
    {
        var entries = ChangeTracker.Entries<IValidateOnSave>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is not { } validatableEntity) continue;
            
            validatableEntity.ValidateBeforeSave();
        }
    }

    /// <summary>
    /// Aktualizuje encje audytowalne (dziedziczące po <see cref="IAuditableEntity"/>)
    /// </summary>
    private void UpdateAuditableEntities()
    {
        var utcNow = DateTime.UtcNow;

        var entries = ChangeTracker.Entries<IAuditableEntity>()
            .Where(x => x.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAtUtc = utcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAtUtc = utcNow;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                case EntityState.Deleted:
                default:
                    break;
            }
        }
    }
}