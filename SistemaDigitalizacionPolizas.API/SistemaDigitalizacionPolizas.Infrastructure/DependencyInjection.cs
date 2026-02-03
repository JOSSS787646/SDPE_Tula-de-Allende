

using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.AdministrtiveUnit;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.AdministrativeUnit_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestingAdministration_Persistences;


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

            //Administrative Unit Repository
            services.AddScoped<IAdministrativeUnit, AdministrativeUnitRepository>();
            //Role Repository
            services.AddScoped<IRoleRepository, RoleRepository>();

            //Permission Repository
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            // Requesting Administration Repository
            services.AddScoped<ICogRepository, CogRepository>();
            //Funding Source Repository
            services.AddScoped<IFundingSourceRepository, FundingSourceRepository>();

            //Servicio de Email
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
            services.AddScoped<IEmailService, EmailService>();



            return services;
        }
    }
}

