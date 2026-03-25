

using SistemaDigitalizacionPolizas.Application.Interfaces;
using SistemaDigitalizacionPolizas.Application.Services.Pdf_Service;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.AdministrtiveUnit;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Community;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.INotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.ISystemConfiguration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Permissions;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestingAdministration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestManager;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Roles;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Suppliers;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Acquisition_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.AdministrativeUnit_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.ApplicationStatus_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Community_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Document_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.NotificationRepository_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.PaymentPolicy_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.RealTime;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestingAdministration_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestManager_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.RequestNotification_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.Auditory;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UnitOfWork;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Supplier_Persistence;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.SystemConfiguration_Persistences;
using SistemaDigitalizacionPolizas.Infrastructure.Services;
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
            //Document Type Repository
            services.AddScoped<IDocumentTypeRepository, DocumentTypeRepository>();
            //Clasification Document Type Repository
            services.AddScoped<IClasificationDocumentTypeRepository, ClasificationDocumentTypeRepository>();
            //Expedient Document Repository
            services.AddScoped<IDocumentExpedientRepository, DocumentExpedientRepository>();
            //Document Status Repository
            services.AddScoped<IDocumentStatusRepository, DocumentStatusRepository>();
            services.AddScoped<IDocumentStatusRepository, DocumentStatusRepository>();
            services.AddScoped<IApplicationDetailRepository, ApplicationDetailRepository>();

            services.AddScoped<IAcquisitionRequest, AcquisitionRequestRepository>();
            services.AddScoped<IRequestDocumentExceptionRepository, RequestDocumentExceptionRepository>();
            services.AddScoped<IRequestManagerRepository, RequestManagerRepository>();
            services.AddScoped<IPaymentPolicyRepository, PaymentPolicyRepository>();
            services.AddScoped<IRequestNotificationRepository, RequestNotificationRepository>();
            services.AddScoped<ISystemConfigurationRepository, SystemConfigurationRepository>();

            services.AddScoped<INotificationRepository, NotificationRepository>();

            services.AddScoped<IPdfService, PdfService>();





            //Application Status Repository
            services.AddScoped<IApplicationStatusRepository, ApplicationStatusRepository>();
            //Servicio de Email
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IPasswordResetRepository, PasswordResetRepository>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUnitOfWorkService, UnitOfWorkService>();
  

            //Servicio de  Auditoria
            services.AddScoped<ICurrentUserService, CurrentUserService>();

            //Servicio de Almacenamiento de Archivos
            services.AddScoped<IFileStorageService, WasabiFileStorageService>();


            services.AddScoped<IRealtimeNotificationService, SignalRNotificationService>();






            return services;
        }
    }
}

