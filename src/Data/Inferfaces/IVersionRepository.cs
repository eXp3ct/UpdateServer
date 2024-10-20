using Domain.Models;

namespace Data.Inferfaces
{
    public interface IVersionRepository : IRepository
    {
        public Task<VersionInfo?> GetLatestVersionAsync(string appName, CancellationToken cancellationToken = default);
        public IQueryable<VersionInfo> GetAllVersionsAsync(string appName, CancellationToken cancellationToken = default);
        public Task<IEnumerable<VersionInfo>> GetAllVersionsAsync(CancellationToken cancellationToken = default);
        public Task AddVersionAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default);
        public Task<VersionInfo?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
