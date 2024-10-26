using AutoMapper;
using Data.Inferfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json", "applicatio/xml")]
    [Consumes("application/json", "applicatio/xml")]
    public class BaseController<TEntity, TEntityDto> : ControllerBase
        where TEntity : class, IEntity
        where TEntityDto : class
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<TEntity> _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<BaseController<TEntity, TEntityDto>> _logger;

        public BaseController(IMapper mapper, IUnitOfWork unitOfWork, ILogger<BaseController<TEntity, TEntityDto>> logger)
        {
            _repository = unitOfWork.Repository<TEntity>();
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        // Получить все
        [HttpGet]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);
            return Ok(entities);
        }

        // Получить по id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity == null)
            {
                return NotFound("Not found");
            }

            return Ok(entity);
        }

        // Создать сущность
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TEntityDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdEntity.Id }, createdEntity);
        }

        // Обновить сущность
        [HttpPut("{id}")]
        public virtual async Task<IActionResult> UpdateAsync(int id, [FromBody] TEntityDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(dto);

            var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
            if (updatedEntity == null)
            {
                return NotFound("Not found");
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Ok(updatedEntity);
        }

        // Удалить сущность
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var deletedEntity = await _repository.DeleteAsync(id, cancellationToken);
            if (deletedEntity == null)
            {
                return NotFound("Not found");
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return NoContent();
        }
    }
}