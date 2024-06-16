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
            .ValueGeneratedOnAdd()
            .HasColumnName("CharacterId");

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

        builder.HasOne(c => c.Campaign)
            .WithMany(c => c.Characters)
            .HasForeignKey(c => c.CampaignId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasData(new List<Character>()
        {
            new Character()
            {
                Id = 1,
                Name = "Aragorn",
                Description = "Dunadan",
                Bio = "Aragorn II, son of Arathorn II and Gilraen, also known as Elessar and Strider, was the 16th Chieftain of the Dúnedain of the North; later crowned King Elessar Telcontar (March 1, 2931 - FO 120 or SR 1541), the 26th King of Arnor and 35th King of Gondor - and first High King of Gondor and Arnor since the short reign of Isildur.",
                Money = 100,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CharacterImageId = 2,
                PlayerOwnerId = 3
            },
            new Character()
            {
                Id = 2,
                Name = "Gandalf",
                Description = "Maiar",
                Bio = "Gandalf, originally named Olórin, was a Maia who served Manwë, Varda, and the Valar, and was sent by them to Middle-earth as the last of the Istari, with the express purpose of advising and assisting all those who opposed the Dark Lord Sauron.",
                Money = 1000,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CharacterImageId = 1
            },
            new Character()
            {
                Id = 3,
                Name = "Joker",
                Description = "Clown Prince of Crime",
                Bio = "The Joker is a master criminal with a clown-like appearance, and is considered one of the most infamous criminals within Gotham City. Initially portrayed as a violent sociopath who murders people for his own amusement, the Joker later in the 1940s began to be written as a goofy trickster-thief.",
                Money = 1000000,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CharacterImageId = 3,
                PlayerOwnerId = 4
            },
            new Character()
            {
                Id = 4,
                Name = "Adolf H",
                Description = "Führer",
                Bio = "Adolf Hitler was an Austrian-born German politician who was the dictator of Germany from 1933 to 1945. He rose to power as the leader of the NSDAP, becoming Chancellor in 1933 and then assuming the title of Führer und Reichskanzler in 1934.",
                Money = 1000000000,
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CharacterImageId = 4
            },
            
        });
    }
}