using MasFinalProj.Domain.Models.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

/// <summary>
/// Konfiguracja encji notatek
/// </summary>
public class NoteConfiguration : IEntityTypeConfiguration<Note>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasKey(n => n.Id);
        
        builder.Property(n => n.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("NoteId");
        
        builder.Property(n => n.Name)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(n => n.Description)
            .IsRequired()
            .HasMaxLength(2000);
        
        builder.HasOne(n => n.CampaignUser)
            .WithMany(cu => cu.Notes)
            .HasForeignKey(n => n.CampaignUserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(n => n.Character)
            .WithMany(c => c.Notes)
            .HasForeignKey(n => n.CharacterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(n => n.CharacterAttribute)
            .WithMany(ca => ca.Notes)
            .HasForeignKey(n => n.CharacterAttributeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}