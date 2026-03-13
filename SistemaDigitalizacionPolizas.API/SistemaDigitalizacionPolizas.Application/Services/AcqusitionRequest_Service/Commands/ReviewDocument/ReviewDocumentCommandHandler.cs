using MediatR;
using Microsoft.Extensions.Logging;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ReviewDocument
{
    public class ReviewDocumentCommandHandler
        : IRequestHandler<ReviewDocumentCommand, bool>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IUnitOfWorkService _unitOfWork;

        private readonly INotificationPolicyService _notificationPolicyService;
        private readonly IRequestNotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        private readonly ILogger<ReviewDocumentCommandHandler> _logger;

        public ReviewDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IRequestStatusService requestStatusService,
            IUnitOfWorkService unitOfWork,
            INotificationPolicyService notificationPolicyService,
            IRequestNotificationRepository notificationRepository,
            IEmailService emailService,
            IUserRepository userRepository,
            ILogger<ReviewDocumentCommandHandler> logger)
        {
            _repository = repository;
            _requestStatusService = requestStatusService;
            _unitOfWork = unitOfWork;

            _notificationPolicyService = notificationPolicyService;
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _userRepository = userRepository;

            _logger = logger;
        }

        public async Task<bool> Handle(
            ReviewDocumentCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando revisión de documento {DocumentId}", request.DocumentId);

            var document = await _repository.GetByIdAsync(request.DocumentId);

            if (document == null)
            {
                _logger.LogWarning("Documento no encontrado {DocumentId}", request.DocumentId);
                throw new Exception("Documento no encontrado.");
            }

            _logger.LogInformation(
                "Documento encontrado: {FileName}, SolicitudId: {RequestId}",
                document.FileName,
                document.RequestId
            );

            if (request.DocumentStatusId == (int)DocumentStatusEnum.Observado &&
                string.IsNullOrWhiteSpace(request.Observations))
            {
                _logger.LogWarning(
                    "Documento observado sin observaciones. DocumentoId {DocumentId}",
                    request.DocumentId
                );

                throw new Exception("Debe agregar observaciones cuando el documento es observado.");
            }

            // =============================
            // ACTUALIZAR DOCUMENTO
            // =============================

            _logger.LogInformation(
                "Actualizando estado documento a {StatusId}",
                request.DocumentStatusId
            );

            document.IdDocumentStatus = request.DocumentStatusId;
            document.Observations = request.Observations;

            await _repository.UpdateAsync(document);

            // =============================
            // RECALCULAR ESTADO SOLICITUD
            // =============================

            _logger.LogInformation(
                "Recalculando estado de solicitud {RequestId}",
                document.RequestId
            );

            await _requestStatusService.RecalculateStatus(document.RequestId);

            // =============================
            // VALIDAR POLÍTICA DE CORREO
            // =============================

            var shouldSendNotification =
                await _notificationPolicyService.ShouldSendNotificationAsync(document.RequestId);

            _logger.LogInformation(
                "Resultado política notificaciones: {ShouldSend}",
                shouldSendNotification
            );

            if (!shouldSendNotification)
            {
                _logger.LogInformation(
                    "Correo bloqueado por política de notificaciones."
                );
                return true;
            }

            // =============================
            // OBTENER REVISOR (ROL READVIEW)
            // =============================

            var reviewerEmail = await _userRepository
                .GetEmailByRoleAsync((int)SystemRolesEnum.ReadView);

            if (string.IsNullOrEmpty(reviewerEmail))
            {
                _logger.LogWarning("No se encontró correo para el rol ReadView.");
                reviewerEmail = "readview@system.local";
            }

            _logger.LogInformation(
                "Revisor identificado como {ReviewerEmail}",
                reviewerEmail
            );

            // ====================================================
            // DOCUMENTO APROBADO
            // ====================================================

            if (request.DocumentStatusId == (int)DocumentStatusEnum.Aprobado)
            {
                _logger.LogInformation("Procesando notificación de documento APROBADO");

                var info = await _notificationRepository
                    .GetDocumentApprovedInfoAsync(document.RequestId);

                if (info == null)
                {
                    _logger.LogWarning(
                        "No se encontró información de notificación para solicitud {RequestId}",
                        document.RequestId
                    );

                    return true;
                }

                _logger.LogInformation(
                    "Datos notificación: RequestNumber={RequestNumber}, ManagerEmail={ManagerEmail}",
                    info.RequestNumber,
                    info.ManagerEmail
                );

                if (!string.IsNullOrEmpty(info.ManagerEmail))
                {
                    _logger.LogInformation(
                        "Enviando correo de documento aprobado a {Email}",
                        info.ManagerEmail
                    );

                    await _emailService.SendDocumentReviewedAsync(
                        info.ManagerEmail,
                        reviewerEmail,
                        info.RequestNumber,
                        document.FileName,
                        "Aprobado",
                        document.Observations
                    );
                }
                else
                {
                    _logger.LogWarning("ManagerEmail viene vacío.");
                }
            }

            // ====================================================
            // DOCUMENTO OBSERVADO
            // ====================================================

            if (request.DocumentStatusId == (int)DocumentStatusEnum.Observado)
            {
                _logger.LogInformation("Procesando notificación de documento OBSERVADO");

                var info = await _notificationRepository
                    .GetDocumentObservedInfoAsync(document.RequestId);

                if (info == null)
                {
                    _logger.LogWarning(
                        "No se encontró información para solicitud {RequestId}",
                        document.RequestId
                    );

                    return true;
                }

                _logger.LogInformation(
                    "Datos notificación: ManagerEmail={ManagerEmail}, AdminEmail={AdminEmail}",
                    info.ManagerEmail,
                    info.AdminEmail
                );

                // correo al encargado
                if (!string.IsNullOrEmpty(info.ManagerEmail))
                {
                    _logger.LogInformation(
                        "Enviando correo al encargado {Email}",
                        info.ManagerEmail
                    );

                    await _emailService.SendDocumentReviewedAsync(
                        info.ManagerEmail,
                        reviewerEmail,
                        info.RequestNumber,
                        document.FileName,
                        "Observado",
                        document.Observations
                    );
                }

                // correo al administrador
                if (!string.IsNullOrEmpty(info.AdminEmail))
                {
                    _logger.LogInformation(
                        "Enviando correo al AdministradorAdquisiciones {Email}",
                        info.AdminEmail
                    );

                    await _emailService.SendDocumentReviewNotificationAsync(
                        info.AdminEmail,
                        reviewerEmail,
                        info.RequestNumber,
                        "Observado",
                        document.Observations
                    );
                }
            }

            _logger.LogInformation(
                "Proceso de revisión finalizado correctamente para documento {DocumentId}",
                request.DocumentId
            );

            return true;
        }
    }
}