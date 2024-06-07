using MasFinalProj.Domain.Models.Campaigns.Characters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

/// <summary>
/// Konfiguracja relacji postaci z innymi postaciami
/// </summary>
public class CharacterRelationWithConfiguration : IEntityTypeConfiguration<CharacterRelationWith>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CharacterRelationWith> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Description)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(c => c.RelationValue)
            .HasPrecision(5, 2);

        builder.HasOne(c => c.FromCharacter)
            .WithMany(c => c.RelationsFromCharacterToOthers)
            .HasForeignKey(c => c.FromCharacterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.ToCharacter)
            .WithMany(c => c.RelationsFromOthersToCharacter)
            .HasForeignKey(c => c.ToCharacterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(c => new { c.FromCharacterId, c.ToCharacterId })
            .IsUnique();

    }
}