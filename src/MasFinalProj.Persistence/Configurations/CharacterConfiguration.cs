using MasFinalProj.Domain.Models.Campaigns.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

/// <summary>
/// Konfiguracja encji postaci
/// </summary>
public class CharacterConfiguration : IEntityTypeConfiguration<Character>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Character> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(52);

        builder.Property(c => c.Description)
            .IsRequired(false)
            .HasMaxLength(1000);
        
        builder.Property(c => c.Bio)
            .IsRequired(false)
            .HasMaxLength(2500);

        builder.Property(c => c.CharacterImageId)
            .IsRequired(false);
        
        builder.Property(c => c.PlayerOwnerId)
            .IsRequired(false);
        
        builder.Property(c => c.Money)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.HasOne(c => c.PlayerOwner)
            .WithMany(p => p.ControlledCharacters)
            .HasForeignKey(c => c.PlayerOwnerId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.HasOne(c => c.CharacterImage)
            .WithMany(i => i.Characters)
            .HasForeignKey(c => c.CharacterImageId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}