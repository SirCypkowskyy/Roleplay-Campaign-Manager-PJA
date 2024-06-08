using MasFinalProj.Domain.Models.Users.New;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations.Users;

/// <summary>
/// Konfiguracja encji zablokowanych emaili
/// </summary>
public class BlacklistedEmailConfiguration : IEntityTypeConfiguration<BlacklistedEmail>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<BlacklistedEmail> builder)
    {
        builder.HasKey(be => be.Id);
        builder.Property(be => be.Id)
            .ValueGeneratedOnAdd();

        builder.HasIndex(be => be.Email)
            .IsUnique();

        builder.Property(be => be.Email)
            .IsRequired()
            .HasMaxLength(52);
    }
}