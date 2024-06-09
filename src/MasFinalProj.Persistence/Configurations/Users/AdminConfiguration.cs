using MasFinalProj.Domain.Helpers;
using MasFinalProj.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations.Users;

/// <summary>
/// Konfiguracja encji administratora
/// </summary>
public class AdminConfiguration : IEntityTypeConfiguration<Admin>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Admin> builder)
    {
        builder.HasBaseType<Moderator>();

        var authData 
            = AuthHelper.GeneratePasswordHash("Test123$");
        
        // Seedowany admin
        builder.HasData(new Admin
        {
            Id = Guid.NewGuid(),
            Username = "BaseAdmin",
            Email = "b.admin@s24759masfinal.com",
            PasswordHash = authData.hashPasswrdBase64,
            PasswordSalt = authData.saltBase64,
            StaffSinceUtc = DateTime.UtcNow,
            Description = "Base admin account",
        });

        builder.HasData(new Admin()
        {
            Id = Guid.NewGuid(),
            Username = "SirCypkowskyy",
            Email = "cypkowski@gmail.com",
            PasswordHash = authData.hashPasswrdBase64,
            PasswordSalt = authData.saltBase64,
            StaffSinceUtc = DateTime.UtcNow,
            Description = "Moje konto do testowania logowania z Discord OAuth",
        });
    }
}