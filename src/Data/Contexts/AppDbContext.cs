using Data.Inferfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<VersionInfo> Versions { get; set; }

    }
}
