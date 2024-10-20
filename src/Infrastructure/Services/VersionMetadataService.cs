using Data.Inferfaces;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services
{
    public class VersionMetadataService : IVersionMetadataService
    {
        private readonly IVersionPathsRepository _repository;

        public VersionMetadataService(IVersionPathsRepository repository)
        {
            _repository = repository;
        }

        public async Task<VersionPaths?> GetVersionPathsAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default)
        {
            return await _repository.GetPathAsync(versionInfo, cancellationToken);
        }

        public async Task<VersionPaths> UpdateVersionPathsAsync(VersionInfo versionInfo, FileType type, string filePath, CancellationToken cancellationToken = default)
        {
            var paths = await _repository.GetPathAsync(versionInfo, cancellationToken);

            if (paths is null)
            {
                paths = new VersionPaths
                {
                    VersionInfoId = versionInfo.Id,
                    ChangelogPath = type == FileType.Changelog ? filePath : string.Empty,
                    ZipPath = type == FileType.Zip ? filePath : string.Empty
                };

                await _repository.AddAsync(paths, cancellationToken);
            }
            else
            {
                if (type == FileType.Changelog)
                    paths.ChangelogPath = filePath;
                else if (type == FileType.Zip)
                    paths.ZipPath = filePath;

                await _repository.UpdateAsync(paths, cancellationToken);
            }

            return paths;
        }
    }
}
