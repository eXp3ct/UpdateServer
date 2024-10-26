using Data.Inferfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Data.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<VersionInfo> Versions { get; set; }
        public DbSet<VersionPath> Paths { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        //public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entries = ChangeTracker.Entries<Application>();

        //    foreach (var entry in entries)
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            entry.Entity.DateOfCreation = DateTime.UtcNow;
        //        }
        //        else if (entry.State == EntityState.Modified)
        //        {
        //            entry.Entity.DateModified = DateTime.UtcNow;
        //        }
        //    }

        //    return await base.SaveChangesAsync(cancellationToken);
        //}
    }
}