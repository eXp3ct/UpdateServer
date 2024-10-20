using Data.Inferfaces;
using Domain.Models;
using Infrastructure.Services.Interfaces;

namespace Infrastructure.Services
{
    public class VersionService : IVersionService
    {
        private readonly IVersionRepository _repository;

        public VersionService(IVersionRepository repository)
        {
            _repository = repository;
        }

        public Task AddVersionAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default)
        {
            return _repository.AddVersionAsync(versionInfo, cancellationToken);
        }

        public async Task<IEnumerable<StoredApplication>> GetAllStoredApplicationsAsync(CancellationToken cancellationToken = default)
        {
            var versions = await _repository.GetAllVersionsAsync(cancellationToken);

            // Группируем и выбираем уникальные приложения по имени
            return versions
                .GroupBy(v => v.ApplicationName)
                .Select(g => new StoredApplication { AppName = g.Key });
        }

        public IEnumerable<VersionInfoShort> GetAllVersionsAsync(string appName)
        {
            return _repository.GetAllVersionsAsync(appName).Select(v => new VersionInfoShort
            {
                Id = v.Id,
                Version = v.Version,
                ReleaseDate = v.ReleaseDate,
                IsMandatory = v.IsMandatory,
            });
        }

        public Task<VersionInfo?> GetLatestVersionAsync(string appName, CancellationToken cancellationToken = default)
        {
            return _repository.GetLatestVersionAsync(appName, cancellationToken);
        }

        public async Task<VersionInfo?> GetVersionAsync(string appName, DateTime? date, CancellationToken cancellationToken = default)
        {

            return date is DateTime versionDate
                ? await GetVersionByDateAsync(appName, versionDate, cancellationToken)
                : await GetLatestVersionAsync(appName, cancellationToken);

        }

        public Task<VersionInfo?> GetVersionByDateAsync(string appName, DateTime date, CancellationToken cancellationToken = default)
        {
            var version = _repository
                .GetAllVersionsAsync(appName, cancellationToken)
                .FirstOrDefault(v => v.ReleaseDate.Date == date.Date);

            return Task.FromResult(version);
        }
    }
}
