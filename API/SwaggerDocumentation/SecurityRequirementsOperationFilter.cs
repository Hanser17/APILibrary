using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.SwaggerDocumentation
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation,
            OperationFilterContext context)
        {
            var hasAuthorize =
                context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any()
                ||
                context.MethodInfo.DeclaringType?
                    .GetCustomAttributes(true)
                    .OfType<AuthorizeAttribute>()
                    .Any() == true;


            var hasAllowAnonymous =
                context.MethodInfo
                    .GetCustomAttributes(true)
                    .OfType<AllowAnonymousAttribute>()
                    .Any();


            if (hasAuthorize && !hasAllowAnonymous)
            {
                operation.Security = new List<OpenApiSecurityRequirement>
            {
                new()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                }
            };
            }
        }
    }
}
