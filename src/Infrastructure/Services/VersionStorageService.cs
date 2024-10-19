using Domain.Enums;
using Domain.Models;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class VersionStorageService : IVersionStorageService
    {
        private readonly IFileStorageService _fileService;
        private readonly IVersionMetadataService _metadataService;

        public VersionStorageService(
            IFileStorageService fileService,
            IVersionMetadataService metadataService)
        {
            _fileService = fileService;
            _metadataService = metadataService;
        }

        public async Task<VersionPaths> SaveVersionFileAsync(IFormFile file, VersionInfo versionInfo, FileType type, CancellationToken cancellationToken = default)
        {
            var folder = Path.Combine(versionInfo.ApplicationName, versionInfo.ReleaseDate.ToString("yyyy-MM-dd"));
            var fileName = type == FileType.Changelog ? "changelog.md" : "release.zip";

            // Сохраняем файл на диск
            var filePath = await _fileService.SaveFileAsync(file, folder, fileName, cancellationToken);

            // Обновляем или создаем запись путей в базе данных
            return await _metadataService.UpdateVersionPathsAsync(versionInfo, type, filePath, cancellationToken);
        }
    }
}
