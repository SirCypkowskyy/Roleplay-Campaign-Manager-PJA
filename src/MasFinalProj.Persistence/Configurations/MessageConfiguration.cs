using MasFinalProj.Domain.Models.Campaigns;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

/// <summary>
/// Konfiguracja encji Message
/// </summary>
public class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.Content).IsRequired();

        builder.Property(m => m.CreatedAtUtc).IsRequired();

        builder.HasOne(m => m.Campaign)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.CharacterAuthor)
            .WithMany(ch => ch.Messages)
            .HasForeignKey(m => m.CharacterAuthorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(m => m.Author)
            .WithMany(u => u.Messages)
            .HasForeignKey(m => m.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}