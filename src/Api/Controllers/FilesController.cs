using Data.Inferfaces;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVersionStorageService _versionStorage;
        private readonly IConfiguration _configuration;

        public FilesController(IUnitOfWork unitOfWork, IVersionStorageService versionStorage, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _versionStorage = versionStorage;
            _configuration = configuration;
        }

        [HttpPost("{versionId}")]
        public async Task<IActionResult> UploadFile(
            [FromRoute] int versionId,
            [FromQuery] FileType type,
            IFormFile file,
            CancellationToken cancellationToken)
        {
            var version = await _unitOfWork.VersionRepository.GetByIdAsync(versionId, cancellationToken);
            if (version is null) return NotFound("Version not found");

            var paths = await _versionStorage.SaveVersionFileAsync(file, version, type, cancellationToken);

            var locationUrl = GenerateFileUrl(versionId, type);
            if (locationUrl is null) return StatusCode(500, "Failed to generate resource URL");

            UpdateVersionUrl(version, type, locationUrl);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Created(locationUrl, paths);
        }

        [HttpGet("{versionId}")]
        public async Task<IActionResult> GetRelease([FromRoute] int versionId, CancellationToken cancellationToken)
        {
            var versionInfo = await _unitOfWork.VersionRepository.GetByIdAsync(versionId, cancellationToken);
            if (versionInfo is null) return NotFound("Version not found");

            var fileBytes = await _versionStorage.ReadVersionFileAsync(versionInfo, FileType.Zip, cancellationToken);
            if (fileBytes is null) return NotFound("File not found");

            return File(fileBytes, "application/zip", _configuration["DefaultReleaseFileName"]);
        }

        [HttpGet("{versionId}/changelog")]
        public async Task<IActionResult> GetChangelog([FromRoute] int versionId, CancellationToken cancellationToken)
        {
            var versionInfo = await _unitOfWork.VersionRepository.GetByIdAsync(versionId, cancellationToken);
            if (versionInfo is null) return NotFound("Version not found");

            var htmlContent = await ReadFileAsString(versionInfo, FileType.Changelog, cancellationToken);
            if (htmlContent is null) return NotFound("File not found");

            return Content(htmlContent, "text/html");
        }

        [HttpDelete("{versionId}")]
        public async Task<IActionResult> DeleteVersionFromStorage([FromRoute] int versionId, CancellationToken cancellationToken)
        {
            var versionInfo = await _unitOfWork.VersionRepository.GetByIdAsync(versionId, cancellationToken);
            if (versionInfo is null) return NotFound("Version not found");

            await _versionStorage.DeleteVersionFilesAsync(versionInfo, cancellationToken);


            return NoContent();
        }

        private string? GenerateFileUrl(int versionId, FileType type)
        {
            return type switch
            {
                FileType.Changelog => Url.Action(nameof(GetChangelog), "Files", new { versionId }, Request.Scheme),
                FileType.Zip => Url.Action(nameof(GetRelease), "Files", new { versionId }, Request.Scheme),
                _ => null
            };
        }

        private static void UpdateVersionUrl(VersionInfo versionInfo, FileType type, string url)
        {
            switch (type)
            {
                case FileType.Changelog:
                    versionInfo.ChangelogUrl = url;
                    break;
                case FileType.Zip:
                    versionInfo.ReleaseUrl = url;
                    break;
            }
        }
        private async Task<string?> ReadFileAsString(VersionInfo versionInfo, FileType type, CancellationToken cancellationToken)
        {
            var fileBytes = await _versionStorage.ReadVersionFileAsync(versionInfo, type, cancellationToken);
            return fileBytes is not null ? Encoding.UTF8.GetString(fileBytes) : null;
        }

    }
}
