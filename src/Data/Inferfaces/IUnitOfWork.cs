using Domain.Interfaces;
using Domain.Models;

namespace Data.Inferfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IApplicationRepository ApplicationRepository { get; }
        public IBaseRepository<VersionInfo> VersionRepository { get; }

        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}