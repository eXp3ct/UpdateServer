using Domain.Models;
using FluentValidation;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VersionsController : ControllerBase
    {
        private readonly IVersionService _versionService;
        private readonly IValidator<VersionInfo> _validator;

        public VersionsController(IVersionService versionChecker, IValidator<VersionInfo> validator)
        {
            _versionService = versionChecker;
            _validator = validator;
        }

        [HttpGet("{appName}")]
        [ProducesResponseType(typeof(VersionInfo), StatusCodes.Status200OK, "application/json", "application/xml")]
        public async Task<IActionResult> GetVersion(
            string appName,
            CancellationToken cancellationToken,
            [FromHeader(Name = "Accept")] string accept = "application/xml",
            [FromQuery] DateTime? date = null)
        {
            var version = await _versionService.GetVersionAsync(appName, date, cancellationToken);

            return version is null
                ? NotFound("Version not found")
                : accept.Contains("application/xml")
                ? new ObjectResult(version)
                {
                    ContentTypes = { "application/xml" },
                    StatusCode = StatusCodes.Status200OK
                }
                : Ok(version);
        }

        [HttpGet("{appName}/list")]
        public IActionResult GetAllVersions([FromRoute] string appName)
        {
            return Ok(_versionService.GetAllVersionsAsync(appName));
        }

        [HttpPost]
        public async Task<IActionResult> CreateVersion([FromBody] VersionInfo version, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(version, cancellationToken);
            if (!validationResult.IsValid)
                return BadRequest("Incorrect version format, must be like '1.1.1.1'");

            await _versionService.AddVersionAsync(version, cancellationToken);
            return CreatedAtAction(nameof(GetVersion), new { appName = version.ApplicationName }, version);
        }

        [HttpGet("apps")]
        public async Task<IActionResult> GetStoredApps(CancellationToken cancellationToken)
        {
            var apps = await _versionService.GetAllStoredApplicationsAsync(cancellationToken);
            return Ok(apps);
        }

        [HttpGet("{versionId}/info")]
        public async Task<IActionResult> GetVersionById([FromRoute] Guid versionId, CancellationToken cancellationToken)
        {
            var versionInfo = await _versionService.GetVersionById(versionId, cancellationToken);

            if (versionInfo is null) return NotFound("Version not found");

            return Ok(versionInfo);
        }
    }
}
