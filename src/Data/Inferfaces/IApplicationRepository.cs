using Domain.Models;

namespace Data.Inferfaces
{
    public interface IApplicationRepository : IBaseRepository<Application>
    {
        public Task<Application?> GetApplicationByNameAsync(string name, CancellationToken cancellationToken);
    }
}