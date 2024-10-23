using Domain.Models;

namespace Infrastructure.Services.Interfaces
{
    public interface IVersionService
    {
        public Task<VersionInfo?> GetLatestVersionAsync(string appName, CancellationToken cancellationToken = default);
        public Task<VersionInfo?> GetVersionByDateAsync(string appName, DateTime date, CancellationToken cancellationToken = default);
        public Task<VersionInfo?> GetVersionAsync(string appName, DateTime? date, CancellationToken cancellationToken = default);
        public Task<VersionInfo?> GetVersionByIdAsync(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<StoredApplication>> GetAllStoredApplicationsAsync(CancellationToken cancellationToken = default);
        public IEnumerable<VersionInfoShort> GetAllVersionsAsync(string appName);
        public Task AddVersionAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default);
        public Task DeleteVersionById(Guid id, CancellationToken cancellationToken = default);

    }
}
