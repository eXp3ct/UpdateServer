using AutoMapper;
using Data.Inferfaces;
using Domain.Dtos;
using Domain.Models;
using FluentValidation;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class VersionController : BaseController<VersionInfo, VersionInfoDto>
    {
        private readonly IVersionInfoRepository _versionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public VersionController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<BaseController<VersionInfo, VersionInfoDto>> logger,
            IValidator<VersionInfo> validator) : base(mapper, unitOfWork, logger, validator)
        {
            _versionRepository = unitOfWork.VersionRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("latest/{appName}")]
        public async Task<IActionResult> GetLatestVersionAsync([FromRoute] string appName, CancellationToken cancellationToken)
        {
            var app = await _unitOfWork.ApplicationRepository.GetApplicationByNameAsync(appName, cancellationToken);

            if (app is null) return NotFound("Application not found");

            var version = await _versionRepository.GetLatestVersionAsync(app, cancellationToken);

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
            if (!result.IsValid) return BadRequest(result.Errors.Select(x => x.ErrorMessage));

            var app = await _unitOfWork.ApplicationRepository.GetApplicationByNameAsync(appName, cancellationToken);
            if (app is null) return NotFound("Application not found");

            var versionInfo = await _versionRepository.GetVersionByString(app, version, cancellationToken);

            return Ok(versionInfo);
        }
    }
}