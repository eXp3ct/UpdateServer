using Data.Inferfaces;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        protected readonly IAppDbContext _context;
        protected readonly DbSet<TEntity> _set;
        private readonly ILogger<BaseRepository<TEntity>> _logger;

        private readonly string _entityName = typeof(TEntity).Name;

        public BaseRepository(IAppDbContext context, ILogger<BaseRepository<TEntity>> logger)
        {
            _context = context;
            _logger = logger;
            _set = _context.Set<TEntity>();
        }

        public async Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entry = await _set.AddAsync(entity, cancellationToken);

            _logger.LogInformation("{type} : {id} was added to database", _entityName, entity.Id);

            return entry.Entity;
        }

        public async Task<TEntity?> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _set.FindAsync([id, cancellationToken], cancellationToken);

            if (entity is null) return null;

            var entry = _set.Remove(entity);

            _logger.LogInformation("{type} : {id} was deleted from database", _entityName, entity.Id);

            return entry.Entity;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var entities = await _set.ToListAsync(cancellationToken);

            _logger.LogInformation("Returning {count} of {type}", entities.Count, _entityName);

            return entities;
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _set.FindAsync([id, cancellationToken], cancellationToken);

            if (entity is null) return null;

            _logger.LogInformation("{type} : {id} found in database", _entityName, entity.Id);

            return entity;
        }

        public async Task<TEntity?> UpdateAsync(int id, TEntity newEntity, CancellationToken cancellationToken = default)
        {
            var entity = await _set.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

            if (entity is null) return null;

            newEntity.Id = id;
            _context.Entry(entity).CurrentValues.SetValues(newEntity);

            _logger.LogInformation("{type} : {id} was updated in database", _entityName, entity.Id);

            return entity;
        }
    }
}