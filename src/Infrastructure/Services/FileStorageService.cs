using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _baseFolder;

        public FileStorageService(IConfiguration configuration)
        {
            _baseFolder = configuration["LocalStorage"]
                ?? throw new ArgumentNullException("Cannot read local storage location");
        }

        public Task DeleteFileAsync(string path, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!File.Exists(path))
                {
                    // Просто логируем отсутствие файла, но не выбрасываем исключение
                    return Task.CompletedTask;
                }

                // Получаем директорию версии до удаления файла
                var versionDirectory = Path.GetDirectoryName(path);
                if (versionDirectory == null)
                    throw new DirectoryNotFoundException($"Directory not found for path: {path}");

                // Удаляем файл
                File.Delete(path);

                // Проверяем остались ли файлы в директории версии
                if (Directory.GetFiles(versionDirectory).Length == 0 &&
                    Directory.GetDirectories(versionDirectory).Length == 0)
                {
                    // Если директория версии пуста - удаляем её
                    Directory.Delete(versionDirectory);
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new IOException($"Error deleting file or directory: {path}", ex);
            }
        }

        public async Task<byte[]> ReadFileAsync(string path, CancellationToken cancellationToken = default)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            return await File.ReadAllBytesAsync(path, cancellationToken);
        }

        public async Task<string> SaveFileAsync(IFormFile file, string folder, string fileName, CancellationToken cancellationToken = default)
        {
            var path = Path.Combine(_baseFolder, folder);
            Directory.CreateDirectory(path);

            var filePath = Path.Combine(path, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream, cancellationToken);

            return filePath;
        }

        public Task DeleteEmptyApplicationDirectory(string applicationPath, CancellationToken cancellationToken = default)
        {
            try
            {
                if (!Directory.Exists(applicationPath))
                    return Task.CompletedTask;

                if (Directory.GetFiles(applicationPath, "*", SearchOption.AllDirectories).Length == 0 &&
                    Directory.GetDirectories(applicationPath).Length == 0)
                {
                    Directory.Delete(applicationPath, recursive: true);
                }

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new IOException($"Error deleting application directory: {applicationPath}", ex);
            }
        }
    }
}