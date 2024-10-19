using Data.Inferfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<VersionInfo> Versions { get; set; }
        public DbSet<VersionPaths> Paths { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
