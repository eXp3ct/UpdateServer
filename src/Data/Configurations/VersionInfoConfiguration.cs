using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class VersionInfoConfiguration : IEntityTypeConfiguration<VersionInfo>
    {
        public void Configure(EntityTypeBuilder<VersionInfo> builder)
        {
            builder.ToTable("Versions");

            builder.HasKey(v => v.Id);

            builder.Property(v => v.Version)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(v => v.ReleaseDate)
                .IsRequired();

            builder.Property(v => v.IsMandatory)
                .IsRequired();

            builder.Property(v => v.IsAvailable)
                .IsRequired();

            builder.Property(v => v.ChangelogUrl)
                .HasMaxLength(500);

            builder.Property(v => v.ReleaseUrl)
                .HasMaxLength(500);

            // Связь с таблицей VersionPath (один ко многим)
            builder.HasOne<VersionPath>()
                .WithMany()
                .HasForeignKey(v => v.PathId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Application>()
                .WithMany()
                .HasForeignKey(v => v.ApplicationId);
        }
    }
}