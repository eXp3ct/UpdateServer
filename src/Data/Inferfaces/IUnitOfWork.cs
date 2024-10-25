using Domain.Interfaces;

namespace Data.Inferfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
    }
}
