using Data.Inferfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories
{
    public class VersionPathsRepository : IVersionPathsRepository
    {
        private readonly IAppDbContext _context;

        public VersionPathsRepository(IAppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(VersionPaths paths, CancellationToken cancellationToken = default)
        {
            await _context.Paths.AddAsync(paths, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<VersionPaths?> GetPathAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default)
        {
            return await _context.Paths.FirstOrDefaultAsync(
                x => x.VersionInfoId == versionInfo.Id, cancellationToken);
        }

        public async Task UpdateAsync(VersionPaths paths, CancellationToken cancellationToken = default)
        {
            _context.Paths.Update(paths);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
