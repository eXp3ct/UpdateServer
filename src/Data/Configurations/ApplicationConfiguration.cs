using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class ApplicationConfiguration : IEntityTypeConfiguration<Application>
    {
        public void Configure(EntityTypeBuilder<Application> builder)
        {
            builder.ToTable("Applications");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Description)
                .HasMaxLength(1000);

            builder.Property(a => a.DateOfCreation)
                .HasDefaultValueSql("datetime(current_timestamp, 'localtime')")
                .IsRequired();

            builder.Property(a => a.DateModified)
                .IsRequired(false);

            // Индексы для повышения производительности поиска по имени приложения
            builder.HasIndex(a => a.Name)
                .IsUnique();
        }
    }
}