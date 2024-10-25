using Data.Inferfaces;
using Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Data
{
    public class UnitOfWork(IAppDbContext context, ILoggerFactory loggerFactory) : IUnitOfWork
    {
        private readonly IAppDbContext _context = context;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly Dictionary<Type, object> _repositories = [];

        public void Dispose()
        {
            _context.Dispose();
        }

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

            var logger = _loggerFactory.CreateLogger<BaseRepository<TEntity>>();
            var newRepository = new BaseRepository<TEntity>(_context, logger);
            _repositories[typeof(TEntity)] = newRepository;

            return newRepository;
        }
    }
}
