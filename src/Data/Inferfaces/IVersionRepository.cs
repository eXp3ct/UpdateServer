using Domain.Models;

namespace Data.Inferfaces
{
    public interface IVersionRepository
    {
        public Task<VersionInfo?> GetLatestVersionAsync(string appName, CancellationToken cancellationToken = default);
        public Task AddVersionAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default);
    }
}
