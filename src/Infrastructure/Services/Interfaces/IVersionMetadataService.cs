using Domain.Enums;
using Domain.Models;

namespace Infrastructure.Services.Interfaces
{
    public interface IVersionMetadataService
    {
        public Task<VersionPaths> UpdateVersionPathsAsync(VersionInfo versionInfo, FileType type, string filePath, CancellationToken cancellationToken = default);
    }
}
