using Domain.Enums;
using Domain.Models;

namespace Data.Inferfaces
{
    public interface IVersionPathsRepository
    {
        public Task AddAsync(VersionPaths versionPath, CancellationToken cancellationToken = default);
        public Task<VersionPaths?> GetPathAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default);
        public Task UpdateAsync(VersionPaths paths, CancellationToken cancellationToken = default);
    }
}
