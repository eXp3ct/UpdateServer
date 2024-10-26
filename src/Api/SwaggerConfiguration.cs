using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api
{
    public class SwaggerConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        public void Configure(SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Version API", Version = "v1" });
            //options.OperationFilter<AcceptHeaderOperationFilter>();
        }
    }

    public class AcceptHeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            //var produces = context.ApiDescription.SupportedResponseTypes
            //    .SelectMany(r => r.ApiResponseFormats)
            //    .Select(f => f.MediaType)
            //    .Distinct();

            //if (produces.Any())
            //{
            //    operation.Parameters ??= [];

            //    operation.Parameters.Add(new OpenApiParameter
            //    {
            //        Name = "Accept",
            //        In = ParameterLocation.Header,
            //        Required = false,
            //        Schema = new OpenApiSchema
            //        {
            //            Type = "string",
            //            Enum = [.. produces.Select(p => new OpenApiString(p))]
            //        }
            //    });
            //}
        }
    }
}