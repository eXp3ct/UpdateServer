using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Interfaces
{
    public interface IFileStorageService
    {
        public Task<string> SaveFileAsync(IFormFile file, string folder, string fileName, CancellationToken cancellationToken = default);
        public Task<byte[]> ReadFileAsync(string path, CancellationToken cancellationToken = default);
        public Task DeleteFileAsync(string path, CancellationToken cancellationToken = default);
    }
}
