using MasFinalProj.Domain.Models.Campaigns.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

/// <summary>
/// Konfiguracja encji CharacterAttribute
/// </summary>
public class CharacterAttributeConfiguration : IEntityTypeConfiguration<CharacterAttribute>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CharacterAttribute> builder)
    {
        builder.HasKey(ca => ca.Id);
        
        builder.Property(ca => ca.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("CharacterAttributeId");
        
        builder.Property(ca => ca.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(ca => ca.Description)
            .HasMaxLength(500);

        builder.HasOne(ca => ca.Character)
            .WithMany(c => c.CharacterAttributes)
            .HasForeignKey(ca => ca.CharacterId);
    }
}