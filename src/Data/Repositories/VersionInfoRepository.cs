using Data.Inferfaces;
using Domain.Models;
using Infrastructure.Compares;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class VersionInfoRepository : BaseRepository<VersionInfo>, IVersionInfoRepository
    {
        public VersionInfoRepository(IAppDbContext context, ILogger<BaseRepository<VersionInfo>> logger) : base(context, logger)
        {
        }

        public async Task<VersionInfo?> GetLatestVersionAsync(Application app, CancellationToken cancellationToken = default)
        {
            var version = (await base.GetAllAsync(cancellationToken))
                .Where(v => v.ApplicationId == app.Id && v.IsAvailable == true)
                .OrderByDescending(v => v.Version, new VersionNumberComparer())
                .FirstOrDefault();

            return version;
        }

        public async Task<VersionInfo?> GetVersionByString(Application app, string version, CancellationToken cancellationToken = default)
        {
            var verionInfo = (await base.GetAllAsync(cancellationToken))
                .Where(v => v.ApplicationId == app.Id && v.IsAvailable == true)
                .FirstOrDefault(v => v.Version == version);

            return verionInfo;
        }
    }
}
