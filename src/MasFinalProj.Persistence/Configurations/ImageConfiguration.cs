using MasFinalProj.Domain.Models.Common;
using MasFinalProj.Persistence.Helpers;
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
        
        var image1ImgAndChecksum = SeedDataHelper.ImageToBase64("./Images/1.png");
        var image2ImgAndChecksum = SeedDataHelper.ImageToBase64("./Images/2.png");
        var image3ImgAndChecksum = SeedDataHelper.ImageToBase64("./Images/3.png");
        var image4ImgAndChecksum = SeedDataHelper.ImageToBase64("./Images/4.png");
        var image5ImgAndChecksum = SeedDataHelper.ImageToBase64("./Images/5.png");
        builder.HasData(new List<Image>()
        {
            new Image()
            {
                Id = 1,
                ImageName = "Aragorn",
                Base64Image = image1ImgAndChecksum.base64Img,
                ImageFormat = "png",
                Checksum = image1ImgAndChecksum.checksum
            },
            new Image()
            {
                Id = 2,
                ImageName = "Gandalf",
                Base64Image = image2ImgAndChecksum.base64Img,
                ImageFormat = "png",
                Checksum = image2ImgAndChecksum.checksum
            },
            new Image()
            {
                Id = 3,
                ImageName = "Joker",
                Base64Image = image3ImgAndChecksum.base64Img,
                ImageFormat = "png",
                Checksum = image3ImgAndChecksum.checksum
            },
            new Image()
            {
                Id = 4,
                ImageName = "Adolf",
                Base64Image = image4ImgAndChecksum.base64Img,
                ImageFormat = "png",
                Checksum = image4ImgAndChecksum.checksum
            },
            new Image()
            {
                Id = 5,
                ImageName = "Columbus Brewing Company",
                Base64Image = image5ImgAndChecksum.base64Img,
                ImageFormat = "png",
                Checksum = image5ImgAndChecksum.checksum
            }
        });
    }
}