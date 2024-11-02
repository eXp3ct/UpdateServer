using Domain.Interfaces;

namespace Data.Inferfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IApplicationRepository ApplicationRepository { get; }
        public IVersionInfoRepository VersionRepository { get; }
        public IVersionPathRepository PathsRepository { get; }

        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}