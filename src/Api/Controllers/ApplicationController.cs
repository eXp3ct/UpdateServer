using AutoMapper;
using Data.Inferfaces;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ApplicationController : BaseController<Application, ApplicationDto>
    {
        private readonly IMapper _mapper;
        private readonly IBaseRepository<Application> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationController(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            ILogger<BaseController<Application, ApplicationDto>> logger) : base(mapper, unitOfWork, logger)
        {
            _mapper = mapper;
            _repository = unitOfWork.Repository<Application>();
            _unitOfWork = unitOfWork;
        }

        public override async Task<IActionResult> UpdateAsync(int id, [FromBody] ApplicationDto dto, CancellationToken cancellationToken)
        {
            var application = await _repository.GetByIdAsync(id, cancellationToken);

            if (application is null) return NotFound("Not found");

            var entity = _mapper.Map<Application>(dto, opt =>
            {
                opt.AfterMap((obj, app) =>
                {
                    app.DateModified = DateTime.Now;
                    app.DateOfCreation = application.DateOfCreation;
                });
            });

            var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
            if (updatedEntity == null)
            {
                return NotFound("Not found");
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updatedEntity);
        }
    }
}