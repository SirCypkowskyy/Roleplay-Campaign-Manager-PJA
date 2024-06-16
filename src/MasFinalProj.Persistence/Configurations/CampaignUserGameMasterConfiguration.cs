using MasFinalProj.Domain.Models.Campaigns.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

/// <summary>
/// Konfiguracja encji CampaignUserGameMaster
/// </summary>
public class CampaignUserGameMasterConfiguration : IEntityTypeConfiguration<CampaignUserGameMaster>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CampaignUserGameMaster> builder)
    {
        builder.HasBaseType<CampaignUser>();
        
        builder.Property(cugm => cugm.Storyline)
            .HasMaxLength(5000);

        builder.HasOne(cugm => cugm.User)
            .WithMany(u => u.CampaignsAsGameMaster)
            .HasForeignKey(cugm => cugm.UserId);

        builder.HasOne(cugm => cugm.Campaign)
            // pomimo, że jest to relacja 1 do 1, musimy użyć WithMany, ponieważ korzystamy z dziedziczenia
            .WithMany(c => c.CampaignGameMaster);

        builder.HasData(new List<CampaignUserGameMaster>()
        {
            new CampaignUserGameMaster()
            {
                Id = 1,
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Storyline = "Test Storyline",
            },
            new CampaignUserGameMaster()
            {
                Id = 2,
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                CampaignId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Storyline = "Test Storyline 2",
            },
        });
    }
}