using Data.Contexts;
using Data.Inferfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data
{
    public static class DependencyInjection
    {
        public static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'Default Connection' not found");

            services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IVersionRepository, VersionRepository>();
            services.AddScoped<IVersionPathsRepository, VersionPathsRepository>();
        }
    }
}
