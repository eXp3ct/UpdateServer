using Data.Inferfaces;
using Data.Repositories;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IAppDbContext _context;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<Type, object> _repositories = [];

        public UnitOfWork(IAppDbContext context, IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public IApplicationRepository ApplicationRepository
            => _serviceProvider.GetRequiredService<IApplicationRepository>();

        public IBaseRepository<VersionInfo> VersionRepository
            => _serviceProvider.GetRequiredService<IBaseRepository<VersionInfo>>();

        // Другие репозитории могут быть добавлены аналогичным образом

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        IBaseRepository<TEntity> IUnitOfWork.Repository<TEntity>()
        {
            if (_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                return (IBaseRepository<TEntity>)repository;
            }

            var logger = new LoggerFactory().CreateLogger<BaseRepository<TEntity>>();
            var newRepository = default(IBaseRepository<TEntity>);

            newRepository = new BaseRepository<TEntity>(_context, logger);
            _repositories[typeof(TEntity)] = newRepository;

            return newRepository;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}