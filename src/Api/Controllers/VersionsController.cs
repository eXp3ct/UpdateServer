using Domain.Models;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VersionsController : ControllerBase
    {
        private readonly IVersionService _versionsChecker;

        public VersionsController(IVersionService versionChecker)
        {
            _versionsChecker = versionChecker;
        }

        [HttpGet("{appName}")]
        [ProducesResponseType(typeof(VersionInfo), StatusCodes.Status200OK, "application/json", "application/xml")]
        public async Task<IActionResult> GetVersion(
            string appName,
            CancellationToken cancellationToken,
            [FromHeader(Name = "Accept")] string accept = "application/xml",
            [FromQuery] DateTime? date = null)
        {
            var version = await _versionsChecker.GetVersion(appName, date, cancellationToken);

            return version is null
                ? NotFound("Version not found")
                : accept.Contains("application/xml")
                ? new ObjectResult(version)
                {
                    ContentTypes = { "application/xml" },
                    StatusCode = StatusCodes.Status200OK
                }
                : (IActionResult)Ok(version);
        }

        [HttpPost]
        public async Task<IActionResult> CreateVersion([FromBody] VersionInfo version, CancellationToken cancellationToken)
        {
            await _versionsChecker.AddVersionAsync(version, cancellationToken);
            return CreatedAtAction(nameof(GetVersion), new { appName = version.ApplicationName }, version);
        }
    }
}
