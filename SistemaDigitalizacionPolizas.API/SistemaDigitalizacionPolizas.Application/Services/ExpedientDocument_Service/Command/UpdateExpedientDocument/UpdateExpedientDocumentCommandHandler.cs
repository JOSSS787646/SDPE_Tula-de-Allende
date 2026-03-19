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
            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
                throw new Exception("Documento no encontrado.");

            string? oldFilePath = entity.FilePath;
            string? newFilePath = null;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // ====================================
                // SUBIR NUEVO ARCHIVO SI SE ENVÍA
                // ====================================
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

                // ====================================
                // ACTUALIZAR OBSERVACIONES
                // ====================================
                if (request.Observations != null)
                    entity.Observations = request.Observations;

                _repository.Update(entity);

                // 🔥 CLAVE: guardar antes del recalculo
                await _unitOfWork.SaveChangesAsync();

                // ====================================
                // RECALCULAR ESTADO
                // ====================================
                await _requestStatusService.RecalculateStatus(entity.RequestId);

                await _unitOfWork.CommitAsync();

                // ====================================
                // ELIMINAR ARCHIVO ANTERIOR
                // ====================================
                if (newFilePath != null && !string.IsNullOrEmpty(oldFilePath))
                    await _fileStorageService.DeleteAsync(oldFilePath);

                // ====================================
                // VALIDAR NOTIFICACIÓN
                // ====================================
                var shouldSendNotification =
                    await _notificationPolicyService.ShouldSendNotificationAsync(entity.RequestId);

                if (!shouldSendNotification)
                    return true;

                var reviewerEmail =
                    await _userRepository.GetEmailByRoleAsync((int)SystemRolesEnum.ReadView);

                if (string.IsNullOrEmpty(reviewerEmail))
                    return true;

                var requestInfo =
                    await _notificationRepository.GetRequestNotificationInfoAsync(entity.RequestId);

                var requestNumber = requestInfo?.RequestNumber ?? "N/A";
                var administrativeUnitName = requestInfo?.AdministrativeUnitName ?? "No especificada";
                var requestDescription = requestInfo?.Justification ?? "Sin justificación";

                // ====================================
                // ENCOLAR CORREO
                // ====================================
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

                // ====================================
                // NOTIFICACIÓN INTERNA
                // ====================================
                var reviewerUserId = await _userRepository
                    .GetUserIdByRoleAsync((int)SystemRolesEnum.ReadView);

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
                await _unitOfWork.RollbackAsync();

                if (newFilePath != null)
                    await _fileStorageService.DeleteAsync(newFilePath);

                throw;
            }
        }
    }
}