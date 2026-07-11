using Microsoft.OpenApi.Models;
using System.Reflection;

namespace API.SwaggerDocumentation
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(
            this IServiceCollection services)
        {
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "1.0",
                    Title = "APILibrary",
                    Description = "API para gestión de Libros y Autores"
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

                opt.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description =
                            "Introduce el token JWT '{token}' SIN usar la parabra bearer"
                    });

                opt.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
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
                    });
            });

            return services;
        }
    }
}
