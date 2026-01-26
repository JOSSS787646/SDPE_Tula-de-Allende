

namespace SistemaDigitalizacionPolizas.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            // DbContext
            services.AddDbContext<SdpeDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection")
                ));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
           

            // JWT
            services.Configure<JwtSettings>(
                configuration.GetSection("Jwt")
            );

            services.AddScoped<IJwtService, JwtService>();

            return services;
        }
    }
}

