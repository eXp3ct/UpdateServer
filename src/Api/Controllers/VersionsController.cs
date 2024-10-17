using Data.Inferfaces;
using Domain.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VersionsController : ControllerBase
    {
        private readonly IVersionRepository _versions;

        public VersionsController(IVersionRepository versions)
        {
            _versions = versions;
        }

        [HttpGet]
        [Route("{appName}")]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> GetLatestVersion(string appName, CancellationToken cancellationToken, [FromHeader(Name = "Accept")] string accept = "application/json")
        {
            var version = await _versions.GetLatestVersionAsync(appName, cancellationToken);

            if (version is null)
                return NotFound("Version not found");

            if (accept.Contains("application/xml"))
                return new ObjectResult(version) { ContentTypes = { "application/xml" } };

            return Ok(version);
        }

        [HttpPost]
        [Produces("application/json", "application/xml")]
        public async Task<IActionResult> CreateVersion([FromBody] VersionInfo version, CancellationToken cancellationToken)
        {
            await _versions.AddVersionAsync(version, cancellationToken);
            return CreatedAtAction(nameof(GetLatestVersion), new { appName = version.ApplicationName }, version);
        }
    }
}
