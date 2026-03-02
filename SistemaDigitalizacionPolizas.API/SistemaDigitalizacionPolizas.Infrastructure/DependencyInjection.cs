

using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Actions;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.AdministrtiveUnit;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Acquisition_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Action_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.AdministrativeUnit_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.ApplicationStatus_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Community_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestingAdministration_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Auditory;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Supplier_Persistence;
using SSistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;


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
            //Proyect Repository
            services.AddScoped<IProyectRepository, ProyectRepository>();
            //Prog Repository
            services.AddScoped<IProgRepository, ProgRepository>();
            //Community Repository
            services.AddScoped<ICommunityRepository, CommunityRepository>();
            //Beneficiary Repository
            services.AddScoped<IBeneficiaryRepository, BeneficiaryRepository>();
            //Acquisition Repository
            services.AddScoped<IAcquisitionRepository, AcquisitionRepository>();
            //Acquisition Classification Repository
            services.AddScoped<IAcquisitionClassificationRepository, AcquisitionClassificationRepository>();
            //Supplier Repository
            services.AddScoped<ISupplierRepository, SupplierRepository>();
            //Action Policy Repository
            services.AddScoped<IActionPolicyRepository, ActionPolicyRepository>();
            //Document Type Repository
            services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            //Clasification Document Type Repository
            services.AddScoped<IClasificationDocumentTypeRepository, ClasificationDocumentTypeRepository>();
            //Expedient Document Repository
            services.AddScoped<IDocumentExpedientRepository, DocumentExpedientRepository>();
            //Document Status Repository
            services.AddScoped<IDocumentStatusRepository, DocumentStatusRepository>();

            services.AddScoped<IAcquisitionRequest, AcquisitionRequestRepository>();

            services.AddScoped<IAcquisitionRequest, AcquisitionRequestRepository>();
            
            //Application Status Repository
            services.AddScoped<IApplicationStatusRepository, ApplicationStatusRepository>();
            //Servicio de Email
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
            services.AddScoped<IEmailService, EmailService>();

            //Servicio de  Auditoria
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            //Servicio de Almacenamiento de Archivos
            services.AddScoped<IFileStorageService, WasabiFileStorageService>();








            return services;
        }
    }
}

