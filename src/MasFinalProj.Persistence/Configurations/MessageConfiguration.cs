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

        builder.Property(m => m.CreatedBy).IsRequired();
        builder.Property(m => m.CreatedAtUtc).IsRequired();
        builder.Property(m => m.ModifiedAtUtc);
        builder.Property(m => m.ModifiedBy);

        builder.HasOne(m => m.Campaign)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.CampaignId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(m => m.CharacterAuthor)
            .WithMany()
            .HasForeignKey(m => m.CharacterAuthorId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(m => m.Author)
            .WithMany()
            .HasForeignKey(m => m.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}