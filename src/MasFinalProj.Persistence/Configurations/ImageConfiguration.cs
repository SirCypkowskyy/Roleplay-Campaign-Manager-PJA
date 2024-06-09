using MasFinalProj.Domain.Models.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations;

/// <summary>
/// Konfiguracja encji Image
/// </summary>
public class ImageConfiguration : IEntityTypeConfiguration<Image>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd()
            .HasColumnName("ImageId");
        
        builder.Property(i => i.ImageName)
            .HasMaxLength(100)
            .IsRequired();
        
        builder.Property(i => i.Base64Image)
            .IsRequired();
        
        builder.Property(i => i.ImageFormat)
            .HasMaxLength(5)
            .IsRequired();
        
        builder.Property(i => i.Checksum)
            .IsRequired();
    }
}