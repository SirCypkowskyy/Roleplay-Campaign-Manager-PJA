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
    }
}