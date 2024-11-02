using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Data.Inferfaces
{
    public interface IAppDbContext : IDisposable
    {
        public DbSet<VersionInfo> Versions { get; }
        public DbSet<VersionPath> Paths { get; }
        public DbSet<Application> Applications { get; }

        public DbSet<T> Set<T>() where T : class;

        public EntityEntry Entry(object entity);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}