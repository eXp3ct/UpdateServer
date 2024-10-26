using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class VersionPathConfiguration : IEntityTypeConfiguration<VersionPath>
    {
        public void Configure(EntityTypeBuilder<VersionPath> builder)
        {
            builder.ToTable("Paths");

            builder.HasKey(vp => vp.Id);

            builder.Property(vp => vp.ChangelogPath)
                .HasMaxLength(500);

            builder.Property(vp => vp.ZipPath)
                .HasMaxLength(500);
        }
    }
}