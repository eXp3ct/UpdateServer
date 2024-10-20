namespace Data.Inferfaces
{
    public interface IRepository
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
