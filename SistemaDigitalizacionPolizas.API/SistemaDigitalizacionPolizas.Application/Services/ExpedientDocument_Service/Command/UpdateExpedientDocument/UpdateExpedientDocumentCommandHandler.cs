using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.CreateNotification;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.UpdateExpedientDocument
{
    public class UpdateExpedientDocumentCommandHandler
       : IRequestHandler<UpdateExpedientDocumentCommand, bool>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IUnitOfWorkService _unitOfWork;

        private readonly INotificationPolicyService _notificationPolicyService;
        private readonly IRequestNotificationRepository _notificationRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailQueue _emailQueue;
        private readonly IMediator _mediator;

        private readonly ICurrentUserService _currentUserService;

        public UpdateExpedientDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IFileStorageService fileStorageService,
            IRequestStatusService requestStatusService,
            IUnitOfWorkService unitOfWork,
            INotificationPolicyService notificationPolicyService,
            IRequestNotificationRepository notificationRepository,
            IEmailService emailService,
            IUserRepository userRepository,
            IEmailQueue emailQueue,
            ICurrentUserService currentUserService,
            IMediator mediator)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
            _requestStatusService = requestStatusService;
            _unitOfWork = unitOfWork;

            _notificationPolicyService = notificationPolicyService;
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _userRepository = userRepository;
            _emailQueue = emailQueue;

            _currentUserService = currentUserService;
            _mediator = mediator;
        }

        public async Task<bool> Handle(
            UpdateExpedientDocumentCommand request,
            CancellationToken cancellationToken)
        {
            // ================================
            // OBTENER DOCUMENTO
            // ================================
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
                throw new Exception("Documento no encontrado.");

            string? oldFilePath = entity.FilePath;
            string? newFilePath = null;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // ================================
                // ACTUALIZACIÓN DE ARCHIVO (OPCIONAL)
                // Si viene archivo nuevo, se reemplaza
                // ================================
                if (request.NewFile != null)
                {
                    await using var stream = request.NewFile.OpenReadStream();

                    newFilePath = await _fileStorageService.UploadAsync(
                        stream,
                        request.NewFile.FileName,
                        request.NewFile.ContentType,
                        "expedientes"
                    );

                    entity.FileName = request.NewFile.FileName;
                    entity.FilePath = newFilePath;
                    entity.UploadDate = DateTime.UtcNow;
                    entity.IdDocumentStatus = (int)DocumentStatusEnum.Cargado;
                }

                // ================================
                // ACTUALIZAR OBSERVACIONES
                // ================================
                if (request.Observations != null)
                    entity.Observations = request.Observations;

                _repository.Update(entity);

                // Importante: persistir antes de recalcular estado
                await _unitOfWork.SaveChangesAsync();

                // ================================
                // REGLA DE NEGOCIO:
                // RECALCULAR ESTADO DE LA SOLICITUD
                // ================================
                await _requestStatusService.RecalculateStatus(entity.RequestId);

                await _unitOfWork.CommitAsync();

                // ================================
                // LIMPIEZA DE ARCHIVO ANTERIOR
                // Solo si se subió uno nuevo
                // ================================
                if (newFilePath != null && !string.IsNullOrEmpty(oldFilePath))
                    await _fileStorageService.DeleteAsync(oldFilePath);

                // ================================
                // OBTENER DATOS PARA NOTIFICACIONES
                // ================================
                var reviewerEmail =
                    await _userRepository.GetEmailByRoleAsync((int)SystemRolesEnum.ReadView);

                var reviewerUserId =
                    await _userRepository.GetUserIdByRoleAsync((int)SystemRolesEnum.ReadView);

                var requestInfo =
                    await _notificationRepository.GetRequestNotificationInfoAsync(entity.RequestId);

                var requestNumber = requestInfo?.RequestNumber ?? "N/A";
                var administrativeUnitName = requestInfo?.AdministrativeUnitName ?? "No especificada";
                var requestDescription = requestInfo?.Justification ?? "Sin justificación";

                // ================================
                // ENVÍO DE CORREO (CONDICIONAL)
                // Depende de política + email válido
                // ================================
                var shouldSendEmail =
                    await _notificationPolicyService.ShouldSendNotificationAsync(entity.RequestId);

                if (shouldSendEmail && !string.IsNullOrEmpty(reviewerEmail))
                {
                    _emailQueue.Enqueue(() =>
                        _emailService.SendDocumentsUploadedAsync(
                            reviewerEmail,
                            _currentUserService.Email,
                            requestNumber,
                            administrativeUnitName,
                            requestDescription,
                            DateTime.Now,
                            $"<div>• Documento actualizado: {entity.FileName}</div>"
                        )
                    );
                }

                // ================================
                // NOTIFICACIÓN (SIEMPRE SE INTENTA)
                // Independiente del correo y política
                // ================================
                if (reviewerUserId > 0)
                {
                    await _mediator.Send(
                        new CreateNotificationCommand(
                            reviewerUserId,
                            "Documento actualizado",
                            $"Se actualizó el documento '{entity.FileName}' en la solicitud {requestNumber}",
                            entity.RequestId
                        ),
                        cancellationToken
                    );
                }

                return true;
            }
            catch
            {
                // ================================
                // ROLLBACK + LIMPIEZA
                // ================================
                await _unitOfWork.RollbackAsync();

                if (newFilePath != null)
                    await _fileStorageService.DeleteAsync(newFilePath);

                throw;
            }
        }
    }
}