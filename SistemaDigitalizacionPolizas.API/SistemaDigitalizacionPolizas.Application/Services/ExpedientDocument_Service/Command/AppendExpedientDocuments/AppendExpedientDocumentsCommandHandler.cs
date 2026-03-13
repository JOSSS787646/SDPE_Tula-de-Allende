using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.AppendExpedientDocuments
{
    /// <summary>
    /// Permite agregar nuevos documentos a un expediente existente.
    ///
    /// Flujo:
    /// 1. Valida archivos enviados
    /// 2. Inicia transacción
    /// 3. Sube archivos al almacenamiento
    /// 4. Crea entidades ExpedientDocument
    /// 5. Guarda en base de datos
    /// 6. Recalcula estado de la solicitud
    /// 7. Confirma transacción
    /// 8. Envía notificación por cola (worker)
    /// </summary>
    public class AppendExpedientDocumentsCommandHandler
        : IRequestHandler<AppendExpedientDocumentsCommand, List<int>>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IDocumentStatusRepository _statusRepository;
        private readonly IRequestNotificationRepository _notificationRepository;
        private readonly INotificationPolicyService _notificationPolicyService;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWorkService _unitOfWork;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;
        private readonly IEmailQueue _emailQueue;

        public AppendExpedientDocumentsCommandHandler(
            IDocumentExpedientRepository repository,
            IDocumentStatusRepository statusRepository,
            IRequestNotificationRepository notificationRepository,
            INotificationPolicyService notificationPolicyService,
            IFileStorageService fileStorageService,
            ICurrentUserService currentUserService,
            IUnitOfWorkService unitOfWork,
            IRequestStatusService requestStatusService,
            IUserRepository userRepository,
            IEmailService emailService,
            IEmailQueue emailQueue)
        {
            _repository = repository;
            _statusRepository = statusRepository;
            _notificationRepository = notificationRepository;
            _notificationPolicyService = notificationPolicyService;
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _requestStatusService = requestStatusService;
            _userRepository = userRepository;
            _emailService = emailService;
            _emailQueue = emailQueue;
        }

        public async Task<List<int>> Handle(
            AppendExpedientDocumentsCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Files == null || !request.Files.Any())
                throw new Exception("Debe enviar al menos un archivo.");

            if (request.Files.Count > 100)
                throw new Exception("Máximo 100 archivos permitidos por carga.");

            var uploadedPaths = new List<string>();
            var entities = new List<ExpedientDocument>();
            var userId = _currentUserService.UserId;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Obtener estado "Cargado"
                var cargadoStatus = await _statusRepository
                    .GetByCodeAsync((int)DocumentStatusEnum.Cargado);

                if (cargadoStatus == null)
                    throw new Exception("No existe estado 'Cargado' configurado.");

                foreach (var file in request.Files)
                {
                    if (file == null || file.Length == 0)
                        throw new Exception("Uno de los archivos enviados es inválido.");

                    await using var stream = file.OpenReadStream();

                    var filePath = await _fileStorageService.UploadAsync(
                        stream,
                        file.FileName,
                        file.ContentType,
                        "expedientes"
                    );

                    uploadedPaths.Add(filePath);

                    entities.Add(new ExpedientDocument
                    {
                        RequestId = request.RequestId,
                        DocumentTypeId = request.DocumentTypeId,
                        IdDocumentStatus = cargadoStatus.idDocumentStatus,
                        FileName = file.FileName,
                        FilePath = filePath,
                        UploadDate = DateTime.UtcNow,
                        Observations = request.Observations,
                        UploadedBy = userId,
                        CreatedBy = userId,
                        CreatedAt = DateTime.UtcNow,
                        Active = true
                    });
                }

                // Guardar documentos
                await _repository.AddRangeAsync(entities);

                // Recalcular estado de la solicitud
                await _requestStatusService.RecalculateStatus(request.RequestId);

                await _unitOfWork.CommitAsync();

                // =====================================
                // VALIDAR NOTIFICACIÓN
                // =====================================

                var shouldSendNotification =
                    await _notificationPolicyService
                        .ShouldSendNotificationAsync(request.RequestId);

                if (!shouldSendNotification)
                    return entities.Select(x => x.Id).ToList();

                var reviewerEmail = await _userRepository
                    .GetEmailByRoleAsync((int)SystemRolesEnum.ReadView);

                if (string.IsNullOrEmpty(reviewerEmail))
                    return entities.Select(x => x.Id).ToList();

                var requestInfo = await _notificationRepository
                    .GetRequestNotificationInfoAsync(request.RequestId);

                var documentList = string.Join("",
                    entities.Select(x => $"<div>• {x.FileName}</div>")
                );

                var requestNumber = requestInfo?.RequestNumber ?? "N/A";
                var administrativeUnitName = requestInfo?.AdministrativeUnitName ?? "No especificada";
                var requestDescription = requestInfo?.Justification ?? "Sin justificación";

                // Enviar correo mediante cola (worker)
                _emailQueue.Enqueue(() =>
                    _emailService.SendDocumentsUploadedAsync(
                        reviewerEmail,
                        _currentUserService.Email,
                        requestNumber,
                        administrativeUnitName,
                        requestDescription,
                        DateTime.Now,
                        documentList
                    )
                );

                return entities.Select(x => x.Id).ToList();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                foreach (var path in uploadedPaths)
                {
                    await _fileStorageService.DeleteAsync(path);
                }

                throw;
            }
        }
    }
}