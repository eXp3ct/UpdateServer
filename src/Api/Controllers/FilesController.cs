using Data.Inferfaces;
using Domain.Enums;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            var versionInfo = await _versionRepository.GetByIdAsync(versionId, cancellationToken);
            if (versionInfo is null) return NotFound();

            var paths = await _versionStorageService.SaveVersionFileAsync(file, versionInfo, type, cancellationToken);
            return Ok(paths);
        }
    }
}
