using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Application.Services.RequestingAdministration_Service.Prog.Queries.GetAllProg;
using SistemaDigitalizacionPolizas.Domain.Entities.RequestingAdministration_Entities;

namespace SistemaDigitalizacionPolizas.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddMediatR(
                typeof(DependencyInjection).Assembly
            );

            services.AddTransient(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehaviour<,>)
            );
            services.AddScoped<IRequestStatusService, RequestStatusService>();
            services.AddScoped<INotificationPolicyService, NotificationPolicyService>();


            return services;
        }
    }
}


