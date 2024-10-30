using Domain.Enums;
using Domain.Models;

namespace Data.Inferfaces
{
    public interface IVersionPathRepository : IBaseRepository<VersionPath>
    {
        public Task<VersionPath> UpdateVersionPathsAsync(int versionId, FileType type, string filePath, CancellationToken cancellationToken = default);
        public Task<VersionPath?> GetVersionPathByVersionId(int versionId, CancellationToken cancellationToken = default);
    }
}
