using MasFinalProj.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasFinalProj.Persistence.Configurations.Users;

/// <summary>
/// Konfiguracja encji Moderator
/// </summary>
public class ModeratorConfiguration : IEntityTypeConfiguration<Moderator>
{   
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Moderator> builder)
    {
        builder.HasBaseType<User>();
        
        builder.Property(m => m.StaffSince)
            .IsRequired();
    }
}