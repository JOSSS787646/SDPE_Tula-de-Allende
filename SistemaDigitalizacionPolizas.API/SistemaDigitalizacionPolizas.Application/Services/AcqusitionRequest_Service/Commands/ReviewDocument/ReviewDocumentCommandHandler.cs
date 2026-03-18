using MediatR;
using Microsoft.Extensions.Logging;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.CreateNotification;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using System.Net.Mail;

namespace SistemaDigitalizacionPolizas.Application.Services.AcqusitionRequest_Service.Commands.ReviewDocument
{
    public class ReviewDocumentCommandHandler
        : IRequestHandler<ReviewDocumentCommand, bool>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IRequestStatusService _requestStatusService;

        private readonly INotificationPolicyService _notificationPolicyService;
        private readonly IRequestNotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailQueue _emailQueue;
        private readonly IMediator _mediator;

        private readonly ILogger<ReviewDocumentCommandHandler> _logger;

        public ReviewDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IRequestStatusService requestStatusService,
            INotificationPolicyService notificationPolicyService,
            IRequestNotificationRepository notificationRepository,
            IEmailService emailService,
            IUserRepository userRepository,
            IEmailQueue emailQueue,
            IMediator mediator,
            ILogger<ReviewDocumentCommandHandler> logger)
        {
            _repository = repository;
            _requestStatusService = requestStatusService;

            _notificationPolicyService = notificationPolicyService;
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _emailQueue = emailQueue;
            _mediator = mediator;

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

            if (request.DocumentStatusId == (int)DocumentStatusEnum.Observado &&
                string.IsNullOrWhiteSpace(request.ObservationsUpload))
            {
                throw new Exception("Debe agregar observaciones cuando el documento es observado.");
            }

            // =====================================
            // ACTUALIZAR DOCUMENTO
            // =====================================

            document.IdDocumentStatus = request.DocumentStatusId;
            document.ObservationsUpload = request.ObservationsUpload;

            await _repository.UpdateAsync(document);

            // =====================================
            // RECALCULAR ESTADO SOLICITUD
            // =====================================

            await _requestStatusService.RecalculateStatus(document.RequestId);

            // =====================================
            // VALIDAR POLÍTICA DE CORREO
            // =====================================

            var shouldSendNotification =
                await _notificationPolicyService.ShouldSendNotificationAsync(document.RequestId);

            if (!shouldSendNotification)
            {
                _logger.LogInformation("Correo bloqueado por política.");
                return true;
            }

            // =====================================
            // DATOS GENERALES
            // =====================================

            var reviewerEmail = await _userRepository
                .GetEmailByRoleAsync((int)SystemRolesEnum.ReadView);

            if (string.IsNullOrEmpty(reviewerEmail))
                reviewerEmail = "readview@system.local";

            var result = request.DocumentStatusId == (int)DocumentStatusEnum.Aprobado
                ? "Aprobado"
                : "Observado";

            string? managerEmail = null;
            string? adminEmail = null;
            string requestNumber = "";

            if (request.DocumentStatusId == (int)DocumentStatusEnum.Aprobado)
            {
                var info = await _notificationRepository
                    .GetDocumentApprovedInfoAsync(document.RequestId);

                if (info == null) return true;

                managerEmail = info.ManagerEmail;
                requestNumber = info.RequestNumber;

                adminEmail = await _userRepository
                    .GetEmailByRoleAsync((int)SystemRolesEnum.AdministradorAdquisiciones);
            }
            else
            {
                var info = await _notificationRepository
                    .GetDocumentObservedInfoAsync(document.RequestId);

                if (info == null) return true;

                managerEmail = info.ManagerEmail;
                adminEmail = info.AdminEmail;
                requestNumber = info.RequestNumber;
            }

            // =====================================
            // ENCOLAR CORREOS
            // =====================================

            if (IsValidEmail(managerEmail))
            {
                _emailQueue.Enqueue(() =>
                    _emailService.SendDocumentReviewedAsync(
                        managerEmail,
                        reviewerEmail,
                        requestNumber,
                        document.FileName,
                        result,
                        document.ObservationsUpload
                ));
            }

            if (IsValidEmail(adminEmail))
            {
                _emailQueue.Enqueue(() =>
                    _emailService.SendDocumentReviewedAsync(
                        managerEmail,
                        reviewerEmail,
                        requestNumber,
                        document.FileName ?? "Documento no identificado",
                        result,
                        document.ObservationsUpload
                ));
            }

            // =====================================
            // 🔥 NOTIFICACIÓN (CORRECTO Y SEPARADO)
            // =====================================

            var adquisicionesUserId = await _userRepository
                .GetUserIdByRoleAsync((int)SystemRolesEnum.AdministradorAdquisiciones);

            if (adquisicionesUserId > 0)
            {
                var statusText = request.DocumentStatusId == (int)DocumentStatusEnum.Aprobado
                    ? "aprobado"
                    : "observado";

                await _mediator.Send(
                    new CreateNotificationCommand(
                        adquisicionesUserId,
                        "Revisión de documento",
                        $"El documento '{document.FileName}' fue {statusText} en la solicitud {requestNumber}",
                        document.RequestId
                    ),
                    cancellationToken
                );
            }

            return true;
        }

        private bool IsValidEmail(string? email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}