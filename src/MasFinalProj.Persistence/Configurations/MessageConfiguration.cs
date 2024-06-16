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
        builder.Property(m => m.Id).ValueGeneratedOnAdd()
            .HasColumnName("MessageId");

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

        builder.HasData(new List<Message>()
        {
            new Message()
            {
                Id = 1,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                AuthorId = 1,
                CharacterAuthorId = 1,
                Content = "Hello, I'm Aragorn",
                CreatedAtUtc = DateTime.UtcNow,
            },
            new Message()
            {
                Id = 2,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                AuthorId = 2,
                CharacterAuthorId = 2,
                Content = "I am Gandalf the Grey",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(1),
            },
            new Message()
            {
                Id = 3,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                AuthorId = 1,
                CharacterAuthorId = 1,
                Content = "We must gather our strength",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(2),
            },
            new Message()
            {
                Id = 4,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                AuthorId = 2,
                Content = "The journey will be perilous",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(3),
            },
            new Message()
            {
                Id = 5,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                AuthorId = 1,
                CharacterAuthorId = 1,
                Content = "We move at dawn",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(4),
            },
            new Message()
            {
                Id = 6,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                AuthorId = 2,
                CharacterAuthorId = 2,
                Content = "I shall lead the way",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(5),
            },
            new Message()
            {
                Id = 7,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                AuthorId = 1,
                CharacterAuthorId = null,
                Content = "The night falls over the land",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(6),
            },
            new Message()
            {
                Id = 8,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                AuthorId = 1,
                CharacterAuthorId = 3,
                Content = "Why so serious?",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(7),
            },
            new Message()
            {
                Id = 9,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                AuthorId = 2,
                CharacterAuthorId = 4,
                Content = "We must secure the future of our people.",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(8),
            },
            new Message()
            {
                Id = 10,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                AuthorId = 1,
                CharacterAuthorId = 3,
                Content = "Let’s put a smile on that face!",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(9),
            },
            new Message()
            {
                Id = 11,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                AuthorId = 2,
                CharacterAuthorId = null,
                Content = "The air is tense at the brewing company.",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(10),
            },
            new Message()
            {
                Id = 12,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                AuthorId = 1,
                CharacterAuthorId = 3,
                Content = "Madness, as you know, is like gravity.",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(11),
            },
            new Message()
            {
                Id = 13,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                AuthorId = 2,
                CharacterAuthorId = 4,
                Content = "Strength lies not in defense but in attack.",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(12),
            },
            new Message()
            {
                Id = 14,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                AuthorId = 1,
                CharacterAuthorId = null,
                Content = "The evening continues with an uneasy silence.",
                CreatedAtUtc = DateTime.UtcNow.AddSeconds(13),
            }
        });

    }
}