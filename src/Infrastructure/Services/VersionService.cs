using Data.Inferfaces;
using Domain.Models;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services
{
    public class VersionService : IVersionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public VersionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<VersionInfo?> GetLatestVersionAsync(string appName, CancellationToken cancellationToken = default)
        {
            var application = await _unitOfWork.ApplicationRepository.GetApplicationByNameAsync(appName, cancellationToken);
            if (application == null)
                return null;

            var version = (await _unitOfWork.VersionRepository.GetAllAsync(cancellationToken))
                .Where(v => v.ApplicationId == application.Id && v.IsAvailable == true)
                .OrderByDescending(v => v.ReleaseDate)
                .FirstOrDefault();

            return version;
        }

        public Task<VersionInfo?> GetVersionByString(string appName, string version, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}