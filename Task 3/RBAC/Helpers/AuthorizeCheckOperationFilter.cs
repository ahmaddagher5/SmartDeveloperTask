using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RBAC.Helpers
{
    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Check if the action or controller has the [Authorize] attribute
            var hasAuthorize = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
                               context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();

            // If the [Authorize] attribute is found, add the security scheme to this operation
            if (hasAuthorize)
            {
                if (operation.Security == null)
                    operation.Security = new List<OpenApiSecurityRequirement>();

                var jwtBearerScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                };

                // Add the Bearer token security requirement
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [jwtBearerScheme] = new List<string>()
                });
            }
            else
            {
                // If no [Authorize] attribute is present, do not add the security scheme
                operation.Security = null;
            }
        }
    }
}
