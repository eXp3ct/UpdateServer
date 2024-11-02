using Data.Inferfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Data.Repositories
{
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(IAppDbContext context, ILogger<BaseRepository<Application>> logger) : base(context, logger)
        {
        }

        public async Task<Application?> GetApplicationByNameAsync(string name, CancellationToken cancellationToken)
        {
            var application = await _set.FirstOrDefaultAsync(v => v.Name == name, cancellationToken);

            return application;
        }
    }
}