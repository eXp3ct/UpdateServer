using Data.Inferfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<VersionInfo?> GetLatestVersionAsync(string appName, CancellationToken cancellationToken = default)
        {
            return await _context.Versions
                .Where(v => v.ApplicationName == appName && v.IsActive)
                .OrderByDescending(v => v.ReleaseDate)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
