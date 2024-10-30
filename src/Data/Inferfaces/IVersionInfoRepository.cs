using Domain.Models;

namespace Data.Inferfaces
{
    public interface IVersionInfoRepository : IBaseRepository<VersionInfo>
    {
        public Task<VersionInfo?> GetLatestVersionAsync(Application app, CancellationToken cancellationToken = default);
        public Task<VersionInfo?> GetVersionByString(Application app, string version, CancellationToken cancellationToken = default);
    }
}
