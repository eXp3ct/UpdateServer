using AutoMapper;
using Data.Inferfaces;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    public class VersionController : BaseController<VersionInfo, VersionInfoDto>
    {
        public VersionController(IBaseRepository<VersionInfo> repository,
            ILogger<BaseController<VersionInfo, VersionInfoDto>> logger,
            IMapper mapper) : base(repository, logger, mapper)
        {
        }
    }
}
