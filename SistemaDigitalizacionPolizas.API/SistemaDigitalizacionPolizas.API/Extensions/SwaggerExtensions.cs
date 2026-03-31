using Microsoft.OpenApi.Models;


namespace SistemaDigitalizacionPolizas.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerIfEnabled(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            if (!configuration.GetValue<bool>("Swagger:Enabled"))
                return services;

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SDPE API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT como: Bearer {token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        public static WebApplication UseSwaggerIfEnabled(
            this WebApplication app,
            IConfiguration configuration)
        {
            if (!configuration.GetValue<bool>("Swagger:Enabled"))
                return app;

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "SDPE API v1");
                options.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}