using MasFinalProj.Domain.Models.Users.New;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations.Users;

/// <summary>
/// Konfiguracja encji odświeżającego tokenu
/// </summary>
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("RefreshTokenId");

        builder.HasIndex(rt => rt.Value)
            .IsUnique();

        builder.Property(rt => rt.Value)
            .IsRequired()
            .HasMaxLength(64);

        builder.Property(rt => rt.ExpiryDateUtc)
            .IsRequired();
        
        builder.HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId);
    }
}