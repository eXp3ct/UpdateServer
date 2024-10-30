using AutoMapper;
using Data.Inferfaces;
using Domain.Dtos;
using Domain.Models;
using FluentValidation;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Api.Controllers
{
    public class VersionController : BaseController<VersionInfo, VersionInfoDto>
    {
        private readonly IVersionService _versionService;

        public VersionController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<BaseController<VersionInfo, VersionInfoDto>> logger,
            IVersionService versionService,
            IValidator<VersionInfo> validator) : base(mapper, unitOfWork, logger, validator)
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

        [HttpGet("{appName}/{version}")]
        public async Task<IActionResult> GetVersionByStringAsync([FromRoute] string appName, [FromRoute] string version, CancellationToken cancellationToken)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotEmpty()
                .NotNull()
                .Matches("^\\d+\\.\\d+\\.\\d+\\.\\d+$")
                .WithMessage("Версия должна быть в формате X.X.X.X");

            var result = await validator.ValidateAsync(version, cancellationToken);
            if(!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage));

            var versionInfo = await _versionService.GetVersionByString(appName, version, cancellationToken);

            if(versionInfo is null) return NotFound();

            return Ok(versionInfo);
        }
    }
}