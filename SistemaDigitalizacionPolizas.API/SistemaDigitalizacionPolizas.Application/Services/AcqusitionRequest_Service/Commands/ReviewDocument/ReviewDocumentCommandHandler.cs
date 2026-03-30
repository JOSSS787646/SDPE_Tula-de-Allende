using MediatR;
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
        private readonly IUnitOfWorkService _unitOfWork;

        private readonly INotificationPolicyService _notificationPolicyService;
        private readonly IRequestNotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailQueue _emailQueue;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public ReviewDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IRequestStatusService requestStatusService,
            IUnitOfWorkService unitOfWork,
            INotificationPolicyService notificationPolicyService,
            IRequestNotificationRepository notificationRepository,
            IEmailService emailService,
            IUserRepository userRepository,
            IEmailQueue emailQueue,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _requestStatusService = requestStatusService;
            _unitOfWork = unitOfWork;

            _notificationPolicyService = notificationPolicyService;
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _emailQueue = emailQueue;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(
            ReviewDocumentCommand request,
            CancellationToken cancellationToken)
        {
            // ================================
            // OBTENER DOCUMENTO
            // ================================
            var document = await _repository.GetByIdAsync(request.DocumentId);

            if (document == null)
                throw new Exception("Documento no encontrado.");

            // ================================
            // VALIDACIÓN DE NEGOCIO
            // ================================
            if (request.DocumentStatusId == (int)DocumentStatusEnum.Observado &&
                string.IsNullOrWhiteSpace(request.ObservationsUpload))
            {
                throw new Exception("Debe agregar observaciones cuando el documento es observado.");
            }

            // ================================
            // ACTUALIZAR DOCUMENTO
            // ================================
            document.IdDocumentStatus = request.DocumentStatusId;
            document.ObservationsUpload = request.ObservationsUpload;

            await _repository.UpdateAsync(document);

            await _unitOfWork.SaveChangesAsync();

            // ================================
            // RECALCULAR ESTADO
            // ================================
            await _requestStatusService.RecalculateStatus(document.RequestId);

            // ================================
            // DATOS PARA NOTIFICACIÓN
            // ================================
            var result = request.DocumentStatusId == (int)DocumentStatusEnum.Aprobado
                ? "Aprobado"
                : "Observado";

            string? managerEmail = null;
            List<string> adminEmails = new();
            string requestNumber = "";

            if (request.DocumentStatusId == (int)DocumentStatusEnum.Aprobado)
            {
                var info = await _notificationRepository
                    .GetDocumentApprovedInfoAsync(document.RequestId);

                if (info != null)
                {
                    managerEmail = info.ManagerEmail;
                    requestNumber = info.RequestNumber;

                    // 🔥 AQUÍ YA TRAES TODOS LOS ADMINS
                    adminEmails = await _userRepository
                        .GetEmailsByRoleAsync((int)SystemRolesEnum.AdministradorAdquisiciones);
                }
            }
            else
            {
                var info = await _notificationRepository
                    .GetDocumentObservedInfoAsync(document.RequestId);

                if (info != null)
                {
                    managerEmail = info.ManagerEmail;
                    requestNumber = info.RequestNumber;

                    // si aquí también quieres múltiples admins:
                    adminEmails = await _userRepository
                        .GetEmailsByRoleAsync((int)SystemRolesEnum.AdministradorAdquisiciones);
                }
            }

            // ================================
            // NORMALIZAR DESTINATARIOS
            // ================================
            var recipients = new List<string>();

            if (IsValidEmail(managerEmail))
                recipients.Add(managerEmail!);

            // 🔥 CLAVE: agregar TODOS los admins
            recipients.AddRange(adminEmails);

            recipients = recipients
                .Where(e => !string.IsNullOrWhiteSpace(e))
                .Distinct()
                .ToList();

            // DEBUG (opcional)
            // Console.WriteLine($"Correos a enviar: {recipients.Count}");

            // ================================
            // ENVÍO DE CORREOS
            // ================================
            var shouldSendEmail =
                await _notificationPolicyService
                    .ShouldSendNotificationAsync(document.RequestId);

            if (shouldSendEmail && recipients.Any())
            {
                foreach (var email in recipients)
                {
                    var capturedEmail = email;

                    _emailQueue.Enqueue(() =>
                        _emailService.SendDocumentReviewedAsync(
                            capturedEmail,
                            _currentUserService.Email,
                            requestNumber,
                            document.FileName ?? "Documento no identificado",
                            result,
                            document.ObservationsUpload
                        )
                    );
                }
            }

            // ================================
            // NOTIFICACIONES IN-APP (MULTI)
            // ================================
            var adquisicionesUserIds = await _userRepository
                .GetUserIdsByRoleAsync((int)SystemRolesEnum.AdministradorAdquisiciones);

            foreach (var userId in adquisicionesUserIds.Where(id => id > 0))
            {
                var statusText = request.DocumentStatusId == (int)DocumentStatusEnum.Aprobado
                    ? "aprobado"
                    : "observado";

                await _mediator.Send(
                    new CreateNotificationCommand(
                        userId,
                        "Revisión de documento",
                        $"El documento '{document.FileName}' fue {statusText} en la solicitud {requestNumber}",
                        document.RequestId
                    ),
                    cancellationToken
                );
            }

            return true;
        }

        // ================================
        // VALIDACIÓN DE EMAIL
        // ================================
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