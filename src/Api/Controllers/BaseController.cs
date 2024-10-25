using AutoMapper;
using Data.Inferfaces;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController<TEntity, TEntityDto> : ControllerBase 
        where TEntity : class, IEntity
        where TEntityDto : class
    {
        private readonly IBaseRepository<TEntity> _repository;
        private readonly ILogger<BaseController<TEntity, TEntityDto>> _logger;
        private readonly IMapper _mapper;

        public BaseController(IBaseRepository<TEntity> repository, ILogger<BaseController<TEntity, TEntityDto>> logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
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
                return NotFound();
            }

            return Ok(entity);
        }

        // Создать сущность
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] TEntityDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var createdEntity = await _repository.CreateAsync(entity, cancellationToken);
            return CreatedAtAction(nameof(GetByIdAsync), new { id = createdEntity.Id }, createdEntity);
        }

        // Обновить сущность
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] TEntityDto dto, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<TEntity>(dto);
            var updatedEntity = await _repository.UpdateAsync(id, entity, cancellationToken);
            if (updatedEntity == null)
            {
                return NotFound();
            }

            return Ok(updatedEntity);
        }

        // Удалить сущность
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var deletedEntity = await _repository.DeleteAsync(id, cancellationToken);
            if (deletedEntity == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
