using Data;
using Infrastructure;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;

        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "VersionUpdater v1");
                c.ConfigObject.AdditionalItems["disableCache"] = true;
            });

            app.UseRouting();
            app.UseCors("AllowAllOrigins");
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfiguration>();
            services.AddControllers()
                .AddNewtonsoftJson()
                .AddXmlSerializerFormatters();
            services.AddEndpointsApiExplorer();
            services.AddPersistance(Configuration);
            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();
            services.AddInfrastructure();
        }
    }
}
