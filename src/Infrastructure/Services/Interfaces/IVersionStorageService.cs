using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Interfaces
{
    public interface IVersionStorageService
    {
        public Task<VersionPaths> SaveVersionFileAsync(IFormFile file, VersionInfo versionInfo, FileType type, CancellationToken cancellationToken = default);
    }
}
