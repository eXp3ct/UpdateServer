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
