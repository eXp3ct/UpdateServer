using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class VersionPathConfiguration : IEntityTypeConfiguration<VersionPaths>
    {
        public void Configure(EntityTypeBuilder<VersionPaths> builder)
        {
            builder.HasKey(vp => vp.VersionInfoId);

            builder.Property(vp => vp.ChangelogPath)
                .HasMaxLength(500);

            builder.Property(vp => vp.ZipPath)
                .HasMaxLength(500);

            builder.HasOne(vp => vp.VersionInfo)
                .WithOne()
                .HasForeignKey<VersionPaths>(vp => vp.VersionInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
