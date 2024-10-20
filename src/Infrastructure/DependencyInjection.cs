using FluentValidation;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IVersionService, VersionService>();
            services.AddScoped<IVersionStorageService, VersionStorageService>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            services.AddScoped<IVersionMetadataService, VersionMetadataService>();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
