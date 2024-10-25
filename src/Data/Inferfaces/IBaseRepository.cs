using Domain.Interfaces;

namespace Data.Inferfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class, IEntity
    {
        public Task<TEntity> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
        public Task<TEntity?> UpdateAsync(int id, TEntity entity, CancellationToken cancellationToken = default);
        public Task<TEntity?> DeleteAsync(int id, CancellationToken cancellationToken = default);
        public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
