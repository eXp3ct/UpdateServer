using AutoMapper;
using Data.Inferfaces;
using Domain.Dtos;
using Domain.Models;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class VersionController : BaseController<VersionInfo, VersionInfoDto>
    {
        private readonly IVersionService _versionService;

        public VersionController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<BaseController<VersionInfo, VersionInfoDto>> logger,
            IVersionService versionService) : base(mapper, unitOfWork, logger)
        {
            _versionService = versionService;
        }

        [HttpGet("latest/{appName}")]
        public async Task<IActionResult> GetLatestVersionAsync([FromRoute] string appName, CancellationToken cancellationToken)
        {
            var version = await _versionService.GetLatestVersionAsync(appName, cancellationToken);

            if (version is null) return NotFound();

            return Ok(version);
        }
    }
}