using Data.Inferfaces;
using Domain.Enums;
using Domain.Models;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesController : ControllerBase
    {
        private readonly IVersionStorageService _versionStorageService;
        private readonly IVersionRepository _versionRepository;

        public FilesController(IVersionStorageService versionStorageService, IVersionRepository versionRepository)
        {
            _versionStorageService = versionStorageService;
            _versionRepository = versionRepository;
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(
            IFormFile file,
            [FromQuery] Guid versionId,
            [FromQuery] FileType type,
            CancellationToken cancellationToken)
        {
            var versionInfo = await GetVersionOrNotFound(versionId, cancellationToken);
            if (versionInfo is null) return NotFound("Version not found");

            var paths = await _versionStorageService.SaveVersionFileAsync(file, versionInfo, type, cancellationToken);

            var locationUrl = GenerateFileUrl(versionId, type);
            if (locationUrl is null) return StatusCode(500, "Failed to generate resource URL");

            UpdateVersionUrl(versionInfo, type, locationUrl);
            await _versionRepository.SaveChangesAsync(cancellationToken);

            return Created(locationUrl, paths);
        }

        [HttpGet("{versionId}/changelog")]
        public async Task<IActionResult> GetChangelog([FromRoute] Guid versionId, CancellationToken cancellationToken)
        {
            var versionInfo = await GetVersionOrNotFound(versionId, cancellationToken);
            if (versionInfo is null) return NotFound("Version not found");

            var htmlContent = await ReadFileAsString(versionInfo, FileType.Changelog, cancellationToken);
            if (htmlContent is null) return NotFound("File not found");

            return Content(htmlContent, "text/html");
        }

        [HttpGet("{versionId}")]
        public async Task<IActionResult> GetRelease([FromRoute] Guid versionId, CancellationToken cancellationToken)
        {
            var versionInfo = await GetVersionOrNotFound(versionId, cancellationToken);
            if (versionInfo is null) return NotFound("Version not found");

            var fileBytes = await _versionStorageService.ReadVersionFileAsync(versionInfo, FileType.Zip, cancellationToken);
            if (fileBytes is null) return NotFound("File not found");

            return File(fileBytes, "application/zip");
        }

        private async Task<VersionInfo?> GetVersionOrNotFound(Guid versionId, CancellationToken cancellationToken)
        {
            return await _versionRepository.GetByIdAsync(versionId, cancellationToken);
        }

        private string? GenerateFileUrl(Guid versionId, FileType type)
        {
            return type switch
            {
                FileType.Changelog => Url.Action(nameof(GetChangelog), "Files", new { versionId }, Request.Scheme),
                FileType.Zip => Url.Action(nameof(GetRelease), "Files", new { versionId }, Request.Scheme),
                _ => null
            };
        }

        private void UpdateVersionUrl(VersionInfo versionInfo, FileType type, string url)
        {
            switch (type)
            {
                case FileType.Changelog:
                    versionInfo.ChangelogFileUrl = url;
                    break;
                case FileType.Zip:
                    versionInfo.ZipUrl = url;
                    break;
            }
        }

        private async Task<string?> ReadFileAsString(VersionInfo versionInfo, FileType type, CancellationToken cancellationToken)
        {
            var fileBytes = await _versionStorageService.ReadVersionFileAsync(versionInfo, type, cancellationToken);
            return fileBytes is not null ? Encoding.UTF8.GetString(fileBytes) : null;
        }
    }
}
