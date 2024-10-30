using Data.Inferfaces;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Extensions;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class VersionStorageService : IVersionStorageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IFileStorageService _fileService;

        public VersionStorageService(IUnitOfWork unitOfWork, IConfiguration configuration, IFileStorageService fileService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _fileService = fileService;
        }

        public async Task DeleteVersionFilesAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default)
        {
            var versionPaths = await _unitOfWork.PathsRepository.GetVersionPathByVersionId(versionInfo.Id, cancellationToken)
                ?? throw new ArgumentNullException($"Version paths not found: {versionInfo.Id}");

            var appDirectory = Path.GetDirectoryName(Path.GetDirectoryName(versionPaths.ZipPath));

            // Удаляем файлы
            await _fileService.DeleteFileAsync(versionPaths.ChangelogPath, cancellationToken);
            await _fileService.DeleteFileAsync(versionPaths.ZipPath, cancellationToken);

            // Проверяем и удаляем директорию приложения, если она пуста
            if (appDirectory != null)
            {
                await _fileService.DeleteEmptyApplicationDirectory(appDirectory, cancellationToken);
            }
        }

        public async Task<byte[]?> ReadVersionFileAsync(VersionInfo versionInfo, FileType type, CancellationToken cancellationToken = default)
        {
            var versionPaths = await _unitOfWork.PathsRepository.GetVersionPathByVersionId(versionInfo.Id, cancellationToken);

            if (versionPaths is null) return null;

            var filePath = type switch
            {
                FileType.Changelog => versionPaths.ChangelogPath,
                FileType.Zip => versionPaths.ZipPath,
                _ => throw new InvalidOperationException(nameof(versionPaths)),
            };

            return await _fileService.ReadFileAsync(filePath, cancellationToken);
        }

        public async Task<VersionPath> SaveVersionFileAsync(IFormFile file, VersionInfo versionInfo, FileType type, CancellationToken cancellationToken = default)
        {
            var app = await _unitOfWork.ApplicationRepository.GetByIdAsync(versionInfo.ApplicationId, cancellationToken);
            var folder = Path.Combine(app.Name, versionInfo.Version.VersionToKebabFormat());
            var fileName = type == FileType.Changelog ? _configuration["DefaultChangelogFileName"] : _configuration["DefaultReleaseFileName"];

            // Сохраняем файл на диск
            var filePath = await _fileService.SaveFileAsync(file, folder, fileName, cancellationToken);

            // Обновляем или создаем запись путей в базе данных
            return await _unitOfWork.PathsRepository.UpdateVersionPathsAsync(versionInfo.Id, type, filePath, cancellationToken);
        }
    }
}
