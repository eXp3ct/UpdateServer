using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Interfaces
{
    public interface IVersionStorageService
    {
        public Task<VersionPath> SaveVersionFileAsync(IFormFile file, VersionInfo versionInfo, FileType type, CancellationToken cancellationToken = default);
        public Task<byte[]?> ReadVersionFileAsync(VersionInfo versionInfo, FileType type, CancellationToken cancellationToken = default);
        public Task DeleteVersionFilesAsync(VersionInfo versionInfo, CancellationToken cancellationToken = default);
    }
}