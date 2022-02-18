using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI_ECommerceSystem.Filters
{
    public class RequireCode : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var classAttributes = context?.MethodInfo?.DeclaringType?.CustomAttributes.Where(a => a.AttributeType == typeof(UseApiKeyAttribute)).FirstOrDefault();
            var methodAttributes = context?.MethodInfo.GetCustomAttributes(true).OfType<UseApiKeyAttribute>().FirstOrDefault();

            var adminMethodAttributes = context?.MethodInfo.GetCustomAttributes(true).OfType<UseAdminKeyAttribute>().FirstOrDefault();

            if (methodAttributes != null || classAttributes != null || adminMethodAttributes != null)
            {
                if(operation.Parameters == null)
                {
                    operation.Parameters = new List<OpenApiParameter>();
                }
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "code",
                    In = ParameterLocation.Query,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }

                });
            }
        }

    }
}
