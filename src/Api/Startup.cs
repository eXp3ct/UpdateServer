using Data;

namespace Api
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; set; } = configuration;

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson()
                .AddXmlSerializerFormatters();
            services.AddEndpointsApiExplorer();
            services.AddPersistance(Configuration);
            services.AddSwaggerGen();
            services.AddSwaggerGenNewtonsoftSupport();
        }
    }
}
