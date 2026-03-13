using MediatR;
using Microsoft.Extensions.Logging;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
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

        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpdateExpedientDocumentCommandHandler> _logger;

        public UpdateExpedientDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IFileStorageService fileStorageService,
            IRequestStatusService requestStatusService,
            IUnitOfWorkService unitOfWork,
            INotificationPolicyService notificationPolicyService,
            IRequestNotificationRepository notificationRepository,
            IEmailService emailService,
            IUserRepository userRepository,
            ICurrentUserService currentUserService,
            ILogger<UpdateExpedientDocumentCommandHandler> logger)
        {
            _repository = repository;
            _fileStorageService = fileStorageService;
            _requestStatusService = requestStatusService;
            _unitOfWork = unitOfWork;

            _notificationPolicyService = notificationPolicyService;
            _notificationRepository = notificationRepository;
            _emailService = emailService;
            _userRepository = userRepository;

            _currentUserService = currentUserService;
            _logger = logger;
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

                if (request.Observations != null)
                    entity.Observations = request.Observations;

                _repository.Update(entity);

                await _requestStatusService.RecalculateStatus(entity.RequestId);

                await _unitOfWork.CommitAsync();

                if (newFilePath != null && !string.IsNullOrEmpty(oldFilePath))
                    await _fileStorageService.DeleteAsync(oldFilePath);

                var shouldSendNotification =
                    await _notificationPolicyService.ShouldSendNotificationAsync(entity.RequestId);

                if (!shouldSendNotification)
                {
                    _logger.LogInformation(
                        "La política bloqueó el envío de correo para RequestId {RequestId}",
                        entity.RequestId);

                    return true;
                }

                var reviewerEmail =
                    await _userRepository.GetEmailByRoleAsync((int)SystemRolesEnum.ReadView);

                if (string.IsNullOrEmpty(reviewerEmail))
                {
                    _logger.LogWarning("No se encontró correo para el rol ReadView.");
                    return true;
                }

                var requestInfo =
                    await _notificationRepository.GetRequestNotificationInfoAsync(entity.RequestId);

                var requestNumber = requestInfo?.RequestNumber ?? "N/A";
                var administrativeUnitName = requestInfo?.AdministrativeUnitName ?? "No especificada";
                var requestDescription = requestInfo?.Justification ?? "Sin justificación";

                await _emailService.SendDocumentsUploadedAsync(
                    reviewerEmail,
                    _currentUserService.Email,
                    requestNumber,
                    administrativeUnitName,
                    requestDescription,
                    DateTime.Now,
                    $"<div>• Documento actualizado: {entity.FileName}</div>"
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