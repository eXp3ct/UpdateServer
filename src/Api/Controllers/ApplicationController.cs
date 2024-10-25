using AutoMapper;
using Data.Inferfaces;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    public class ApplicationController : BaseController<Application, ApplicationDto>
    {
        public ApplicationController(IBaseRepository<Application> repository, 
            ILogger<BaseController<Application, ApplicationDto>> logger, 
            IMapper mapper) : base(repository, logger, mapper)
        {
        }
    }
}
