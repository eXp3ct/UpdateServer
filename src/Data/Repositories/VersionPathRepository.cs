using Data.Inferfaces;
using Domain.Enums;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class VersionPathRepository : BaseRepository<VersionPath>, IVersionPathRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public VersionPathRepository(IAppDbContext context,
                                     ILogger<BaseRepository<VersionPath>> logger,
                                     IUnitOfWork unitOfWork) : base(context, logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VersionPath> UpdateVersionPathsAsync(int versionId, FileType type, string filePath, CancellationToken cancellationToken = default)
        {
            var paths = await GetVersionPathByVersionId(versionId, cancellationToken);


            if (paths is null)
            {
                paths = new VersionPath
                {
                    VersionInfoId = versionId,
                    ChangelogPath = type == FileType.Changelog ? filePath : string.Empty,
                    ZipPath = type == FileType.Zip ? filePath : string.Empty
                };

                await CreateAsync(paths, cancellationToken);
            }
            else
            {
                if (type == FileType.Changelog)
                    paths.ChangelogPath = filePath;
                else if (type == FileType.Zip)
                    paths.ZipPath = filePath;

                await UpdateAsync(paths.Id, paths, cancellationToken);
            }

            return paths;
        }

        public async Task<VersionPath?> GetVersionPathByVersionId(int versionId, CancellationToken cancellationToken = default)
        {
            var version = await _unitOfWork.VersionRepository.GetByIdAsync(versionId, cancellationToken);

            var path = (await GetAllAsync(cancellationToken))
                .FirstOrDefault(x => x.VersionInfoId == version.Id);

            return path;
        }
    }
}
