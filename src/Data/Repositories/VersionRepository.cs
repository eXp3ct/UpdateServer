using Data.Inferfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class VersionRepository : IVersionRepository
    {
        private readonly IAppDbContext _context;

        public VersionRepository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task AddVersionAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default)
        {
            versionInfo.Id = Guid.NewGuid();
            versionInfo.ReleaseDate = DateTime.UtcNow;

            await _context.Versions.AddAsync(versionInfo, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteVersionByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var info = await GetByIdAsync(id, cancellationToken);

            if(info is null)
                return;

            _context.Versions.Remove(info);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public IQueryable<VersionInfo> GetAllVersionsAsync(string appName, CancellationToken cancellationToken = default)
        {
            return _context.Versions.Where(v => v.ApplicationName == appName);
        }

        public async Task<IEnumerable<VersionInfo>> GetAllVersionsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Versions.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<VersionInfo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Versions.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
        }

        public async Task<VersionInfo?> GetLatestVersionAsync(string appName, CancellationToken cancellationToken = default)
        {
            return await _context.Versions
                .Where(v => v.ApplicationName == appName && v.IsActive)
                .OrderByDescending(v => v.ReleaseDate)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }
    }
}
