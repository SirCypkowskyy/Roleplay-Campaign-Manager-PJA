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
            .WithMany()
            .HasForeignKey(u => u.ProfileImageId);
    }
}