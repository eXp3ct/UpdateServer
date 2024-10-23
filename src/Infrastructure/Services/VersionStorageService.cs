using Data.Inferfaces;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class VersionStorageService : IVersionStorageService
    {
        private readonly IFileStorageService _fileService;
        private readonly IVersionMetadataService _metadataService;
        private readonly IVersionRepository _versionRepository;
        private readonly IConfiguration _configuration;

        public VersionStorageService(
            IFileStorageService fileService,
            IVersionMetadataService metadataService,
            IVersionRepository versionRepository,
            IConfiguration configuration)
        {
            _fileService = fileService;
            _metadataService = metadataService;
            _versionRepository = versionRepository;
            _configuration = configuration;
        }

        public async Task DeleteVersionFilesAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default)
        {
            var versionPaths = await _metadataService.GetVersionPathsAsync(versionInfo, cancellationToken)
                ?? throw new ArgumentNullException($"Version paths not found: {versionInfo.Id}");

            await _fileService.DeleteFileAsync(versionPaths.ChangelogPath, cancellationToken);
            await _fileService.DeleteFileAsync(versionPaths.ZipPath, cancellationToken);
        }

        public async Task<byte[]?> ReadVersionFileAsync(VersionInfo versionInfo, FileType type, CancellationToken cancellationToken = default)
        {
            var versionPaths = await _metadataService.GetVersionPathsAsync(versionInfo, cancellationToken);

            if(versionPaths is null) return null;

            var filePath = type switch
            {
                FileType.Changelog => versionPaths.ChangelogPath,
                FileType.Zip => versionPaths.ZipPath,
                _ => throw new InvalidOperationException(nameof(versionPaths)),
            };

            return await _fileService.ReadFileAsync(filePath, cancellationToken);
        }

        public async Task<VersionPaths> SaveVersionFileAsync(IFormFile file, VersionInfo versionInfo, FileType type, CancellationToken cancellationToken = default)
        {
            var folder = Path.Combine(versionInfo.ApplicationName, versionInfo.ReleaseDate.ToString("yyyy-MM-dd"));
            var fileName = type == FileType.Changelog ? _configuration["DefaultChangelogFileName"] : _configuration["DefaultReleaseFileName"];

            // Сохраняем файл на диск
            var filePath = await _fileService.SaveFileAsync(file, folder, fileName, cancellationToken);

            // Обновляем или создаем запись путей в базе данных
            return await _metadataService.UpdateVersionPathsAsync(versionInfo, type, filePath, cancellationToken);
        }
    }
}
