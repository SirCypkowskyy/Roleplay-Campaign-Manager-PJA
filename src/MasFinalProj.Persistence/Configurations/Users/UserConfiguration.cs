using MasFinalProj.Domain.Helpers;
using MasFinalProj.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations.Users;

/// <summary>
/// Konfiguracja encji użytkownika
/// </summary>
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(u => u.Email)
            .IsUnique();
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(52);
        
        builder.HasIndex(u => u.Username)
            .IsUnique();
        
        builder.Property(u => u.Username)
            .IsRequired()
            .HasMaxLength(32);
        
        builder.Property(u => u.DiscordId)
            .IsRequired(false);
        
        builder.Property(u => u.DiscordUsername)
            .IsRequired(false);
        
        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(64);
        
        builder.Property(u => u.PasswordSalt)
            .IsRequired()
            .HasMaxLength(64);
        
        builder.Property(u => u.Description)
            .IsRequired(false)
            .HasMaxLength(2500);
        
        builder.Property(u => u.ProfileImageId)
            .IsRequired(false);

        builder.HasOne(u => u.ProfileImage)
            .WithMany(i => i.UsersWithImage)
            .HasForeignKey(u => u.ProfileImageId);

        var authData = AuthHelper.GeneratePasswordHash("Test123$");
        
        // Seedowany użytkownik
        builder.HasData(new User
        {
            Id = Guid.NewGuid(),
            Username = "BaseUser",
            Email = "user@s24759masfinal.com",
            PasswordHash = authData.hashPasswrdBase64,
            PasswordSalt = authData.saltBase64,
            Description = "Testowy użytkownik",
            CreatedBy = "Seed",
            CreatedAtUtc = DateTime.UtcNow
        });
    }
}