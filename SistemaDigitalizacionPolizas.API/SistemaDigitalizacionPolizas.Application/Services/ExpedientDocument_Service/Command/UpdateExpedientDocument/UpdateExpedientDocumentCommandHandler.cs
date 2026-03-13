using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.UpdateExpedientDocument
{
    /// <summary>
    /// Permite actualizar un documento existente dentro de un expediente.
    ///
    /// Flujo del proceso:
    /// 1. Obtiene el documento existente desde base de datos
    /// 2. Inicia una transacción
    /// 3. Si se envía un nuevo archivo:
    ///     - se sube al almacenamiento
    ///     - se actualiza el nombre y la ruta
    ///     - se cambia el estado del documento a "Cargado"
    /// 4. Actualiza observaciones si se proporcionan
    /// 5. Recalcula el estado de la solicitud
    /// 6. Confirma la transacción
    /// 7. Elimina el archivo anterior si fue reemplazado
    /// 8. Si la política lo permite, envía notificación por cola (worker)
    ///
    /// El correo no se envía directamente, se coloca en EmailQueue
    /// para que el EmailBackgroundWorker lo procese con reintentos.
    /// </summary>
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
            ICurrentUserService currentUserService)
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

                // ====================================
                // RECALCULAR ESTADO DE SOLICITUD
                // ====================================

                await _requestStatusService.RecalculateStatus(entity.RequestId);

                await _unitOfWork.CommitAsync();

                // ====================================
                // ELIMINAR ARCHIVO ANTERIOR
                // ====================================

                if (newFilePath != null && !string.IsNullOrEmpty(oldFilePath))
                    await _fileStorageService.DeleteAsync(oldFilePath);

                // ====================================
                // VALIDAR ENVÍO DE NOTIFICACIÓN
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
                // ENCOLAR CORREO (WORKER)
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