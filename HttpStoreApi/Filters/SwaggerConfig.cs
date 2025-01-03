using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace HttpStoreApi.Filters
{
    public class SwaggerConfig : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var existingParameter = operation.Parameters.FirstOrDefault(p => p.Name == "apiId");
            if (existingParameter != null)
            {
                operation.Parameters.Remove(existingParameter);
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "apiId",
                In = ParameterLocation.Query,
                Required = true,
                Description = "API identifier to specify which API to call",
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<IOpenApiAny>
                    {
                        new OpenApiString("1"),
                        new OpenApiString("2")
                    }
                }
            });
        }
    }
}
