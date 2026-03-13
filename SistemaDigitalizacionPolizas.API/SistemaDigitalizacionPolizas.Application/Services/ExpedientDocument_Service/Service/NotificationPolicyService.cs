
using Microsoft.Extensions.Logging;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.ISystemConfiguration;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service
{
    public class NotificationPolicyService : INotificationPolicyService
    {
        private readonly ISystemConfigurationRepository _configurationRepository;
        private readonly IRequestNotificationRepository _requestRepository;
        private readonly ILogger<NotificationPolicyService> _logger;

        public NotificationPolicyService(
            ISystemConfigurationRepository configurationRepository,
            IRequestNotificationRepository requestRepository,
            ILogger<NotificationPolicyService> logger)
        {
            _configurationRepository = configurationRepository;
            _requestRepository = requestRepository;
            _logger = logger;
        }

        public async Task<bool> ShouldSendNotificationAsync(int requestId)
        {
            _logger.LogInformation("Evaluando política de notificación para RequestId {RequestId}", requestId);

            var config = await _configurationRepository.GetActiveAsync();

            if (config == null)
            {
                _logger.LogWarning("No existe configuración activa del sistema. No se enviará correo.");
                return false;
            }

            if (!config.EmailsEnabled)
            {
                _logger.LogInformation("Los correos están deshabilitados en la configuración del sistema.");
                return false;
            }

            if (!config.NotificationStartDate.HasValue)
            {
                _logger.LogInformation("Correos habilitados sin fecha límite. Se enviará notificación.");
                return true;
            }

            var requestInfo = await _requestRepository
                .GetRequestNotificationInfoAsync(requestId);

            if (requestInfo == null)
            {
                _logger.LogWarning("No se encontró información de solicitud para RequestId {RequestId}", requestId);
                return false;
            }

            var result = requestInfo.RequestDate >= config.NotificationStartDate.Value;

            _logger.LogInformation(
                "Evaluación fecha solicitud: RequestDate={RequestDate}, ConfigStartDate={ConfigStartDate}, Resultado={Result}",
                requestInfo.RequestDate,
                config.NotificationStartDate.Value,
                result
            );

            return result;
        }
    }
}