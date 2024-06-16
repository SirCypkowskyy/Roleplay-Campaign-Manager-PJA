using MasFinalProj.Domain.Models.Campaigns.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

/// <summary>
/// Konfiguracja encji CampaignUserPlayer
/// </summary>
public class CampaignUserPlayerConfiguration : IEntityTypeConfiguration<CampaignUserPlayer>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CampaignUserPlayer> builder)
    {
        builder.HasBaseType<CampaignUser>();

        builder.HasOne(cup => cup.User)
            .WithMany(u => u.CampaignsAsPlayer)
            .HasForeignKey(cup => cup.UserId);

        builder.HasOne(cup => cup.Campaign)
            .WithMany(c => c.CampaignPlayers)
            .HasForeignKey(cup => cup.CampaignId);

        builder.Property(cup => cup.CampaignNickname)
            .HasMaxLength(32);
        
        builder.Property(cup => cup.CampaignBio)
            .HasMaxLength(200);

        builder.HasData(new List<CampaignUserPlayer>()
        {
            new CampaignUserPlayer()
            {
                Id = 3,
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CampaignNickname = "Test Player",
                CampaignBio = "Test Player Bio",
            },
            new CampaignUserPlayer()
            {
                Id = 4,
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CampaignNickname = "Test Player 2",
                CampaignBio = "Test Player Bio 2",
            },
        });
    }
}