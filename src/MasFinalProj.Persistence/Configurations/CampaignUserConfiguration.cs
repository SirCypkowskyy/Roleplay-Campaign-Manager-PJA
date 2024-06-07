using MasFinalProj.Domain.Models.Campaigns.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

public class CampaignUserConfiguration : IEntityTypeConfiguration<CampaignUser>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<CampaignUser> builder)
    {
        builder.HasDiscriminator<string>("CampaignUserType")
            .HasValue<CampaignUser>("CampaignUser")
            .HasValue<CampaignUserPlayer>("CampaignUserPlayer")
            .HasValue<CampaignUserGameMaster>("CampaignUserGameMaster");

        builder.HasKey(cu => cu.Id);
        builder.Property(cu => cu.Id)
            .ValueGeneratedOnAdd();

        // builder.HasOne(cu => cu.User)
        //     .WithMany(u => u.CampaignUsers)
        //     .HasForeignKey(cu => cu.UserId);
        //
        // builder.HasOne(cu => cu.Campaign)
        //     .WithMany(c => c.CampaignUsers)
        //     .HasForeignKey(cu => cu.CampaignId);
    }
}