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
            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            File.Delete(path);
            
            var directory = Path.GetDirectoryName(path);

            if (directory is null)
                throw new DirectoryNotFoundException($"Directory not found: {directory}");

            if(Directory.GetFiles(directory).Length <= 0)
            {
                Directory.Delete(directory);
                var parent = Directory.GetParent(directory);
                Directory.Delete(parent.FullName);
            }

            return Task.CompletedTask;
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
    }
}
