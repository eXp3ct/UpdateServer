using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Inferfaces
{
    public interface IAppDbContext
    {
        public DbSet<VersionInfo> Versions { get; }
        public DbSet<VersionPaths> Paths { get; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
