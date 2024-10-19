using Domain.Models;

namespace Infrastructure.Services.Interfaces
{
    public interface IVersionService
    {
        public Task<VersionInfo?> GetLatestVersionAsync(string appName, CancellationToken cancellationToken = default);
        public Task<VersionInfo?> GetVersionByDateAsync(string appName, DateTime date, CancellationToken cancellationToken = default);
        public Task<VersionInfo?> GetVersion(string appName, DateTime? date, CancellationToken cancellationToken = default);
        public Task AddVersionAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default);

    }
}
