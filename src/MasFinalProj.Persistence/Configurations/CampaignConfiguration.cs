using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

public class CampaignConfiguration : IEntityTypeConfiguration<Campaign>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Campaign> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("CampaignId");

        builder.HasIndex(c => c.Name)
            .IsUnique();
        
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(52);

        builder.Property(c => c.Description)
            .HasMaxLength(2000);

        builder.Property(c => c.IsArchived)
            .HasDefaultValue(false);

        builder.Property(c => c.IsPublic)
            .HasDefaultValue(false);
        
        builder.Property(c => c.GameCurrency)
            .HasMaxLength(100);
        
        builder.HasOne(c => c.CampaignImage)
            .WithMany(i => i.CampaignsWithImage)
            .HasForeignKey(c => c.CampaignImageId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasData(new List<Campaign>()
        {
            new Campaign()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                Name = "Mordor Adventures",
                Description = "Adventures of Gandalf and Aragorn through Mordor",
                GameCurrency = "Ducats",
                IsPublic = false,
                IsArchived = false,
            },
            new Campaign()
            {
                Id = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                Name = "Joker and Adi",
                Description = "A quiet evening with Joker and Adolf at the Columbus Brewing Company",
                GameCurrency = "Gold",
                IsPublic = false,
                IsArchived = false,
                CampaignImageId = 5
            }
        });
    }
}