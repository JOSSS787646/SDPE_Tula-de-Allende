using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;
using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.CreateNotification;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.CreateMassiveExpedientDocument
{
    public class CreateMassiveExpedientDocumentCommandHandler
        : IRequestHandler<CreateMassiveExpedientDocumentCommand, List<int>>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IDocumentStatusRepository _statusRepository;
        private readonly IRequestNotificationRepository _notificationRepository;
        private readonly INotificationPolicyService _notificationPolicyService;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWorkService _unitOfWork;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailQueue _emailQueue;
        private readonly IMediator _mediator;

        public CreateMassiveExpedientDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IDocumentStatusRepository statusRepository,
            IRequestNotificationRepository notificationRepository,
            INotificationPolicyService notificationPolicyService,
            IFileStorageService fileStorageService,
            ICurrentUserService currentUserService,
            IUnitOfWorkService unitOfWork,
            IRequestStatusService requestStatusService,
            IEmailService emailService,
            IUserRepository userRepository,
            IEmailQueue emailQueue,
            IMediator mediator
        )
        {
            _repository = repository;
            _statusRepository = statusRepository;
            _notificationRepository = notificationRepository;
            _notificationPolicyService = notificationPolicyService;
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
            _requestStatusService = requestStatusService;
            _emailService = emailService;
            _userRepository = userRepository;
            _emailQueue = emailQueue;
            _mediator = mediator;
        }

        // ============================================================
        // HANDLER CORREGIDO
        // ============================================================
        public async Task<List<int>> Handle(
            CreateMassiveExpedientDocumentCommand request,
            CancellationToken cancellationToken)
        {
            if (request.Documents == null || !request.Documents.Any())
                throw new Exception("Debe enviar al menos un archivo.");

            if (request.Documents.Count > 100)
                throw new Exception("Máximo 100 archivos permitidos por carga.");

            var uploadedPaths = new List<string>();
            var entities = new List<ExpedientDocument>();
            var userId = _currentUserService.UserId;
            var uploaderEmail = _currentUserService.Email;

            // ====================================================
            // TRANSACCIÓN 1: solo subida de archivos y documentos
            // ====================================================
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var cargadoStatus = await _statusRepository
                    .GetByCodeAsync((int)DocumentStatusEnum.Cargado);

                if (cargadoStatus == null)
                    throw new Exception("No existe estado 'Cargado' configurado.");

                var semaphore = new SemaphoreSlim(3);

                var tasks = request.Documents.Select(async item =>
                {
                    await semaphore.WaitAsync(cancellationToken);
                    try
                    {
                        if (item.File == null || item.File.Length == 0)
                            throw new Exception("Uno de los archivos enviados es inválido.");

                        await using var stream = item.File.OpenReadStream();

                        var filePath = await _fileStorageService.UploadAsync(
                            stream,
                            item.File.FileName,
                            item.File.ContentType,
                            "expedientes"
                        );

                        lock (uploadedPaths)
                            uploadedPaths.Add(filePath);

                        var entity = new ExpedientDocument
                        {
                            RequestId = request.RequestId,
                            DocumentTypeId = item.DocumentTypeId,
                            IdDocumentStatus = (int)DocumentStatusEnum.Cargado,
                            FileName = item.File.FileName,
                            FilePath = filePath,
                            UploadDate = DateTime.UtcNow,
                            Observations = item.Observations,
                            UploadedBy = userId,
                            CreatedBy = userId,
                            CreatedAt = DateTime.UtcNow,
                            Active = true
                        };

                        lock (entities)
                            entities.Add(entity);
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                await Task.WhenAll(tasks);

                await _repository.AddRangeAsync(entities);

                // ✅ COMMIT PRIMERO: los docs ya están en BD
                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();

                foreach (var path in uploadedPaths)
                    await _fileStorageService.DeleteAsync(path);

                throw;
            }

            // ====================================================
            // TRANSACCIÓN 2: recalcular status DESPUÉS del commit
            // Los documentos ya existen en BD, la lectura es correcta
            // ====================================================
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // ✅ Ahora GetActiveByRequestId SÍ ve los docs nuevos
                await _requestStatusService.RecalculateStatus(request.RequestId);

                await _unitOfWork.CommitAsync();
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                // No borrar archivos aquí: los docs YA se guardaron en tx1
                // Solo falla el status, que puede reintentarse
                throw;
            }

            // ====================================================
            // POST-COMMIT: emails y notificaciones (fuera de tx)
            // ====================================================
            var reviewerEmails = await _userRepository
                .GetEmailsByRoleAsync((int)SystemRolesEnum.AdministradorTesoreria);

            var reviewerUserIds = await _userRepository
                .GetUserIdsByRoleAsync((int)SystemRolesEnum.AdministradorTesoreria);

            var requestInfo = await _notificationRepository
                .GetRequestNotificationInfoAsync(request.RequestId);

            var documentList = string.Join("",
                entities.Select(x => $"<div>• {x.FileName}</div>")
            );

            var requestNumber = requestInfo?.RequestNumber ?? "N/A";
            var administrativeUnit = requestInfo?.AdministrativeUnitName ?? "No especificada";
            var requestDescription = requestInfo?.Justification ?? "Sin justificación";

            var shouldSendEmail =
                await _notificationPolicyService.ShouldSendNotificationAsync(request.RequestId);

            if (shouldSendEmail && reviewerEmails.Any())
            {
                foreach (var email in reviewerEmails
                    .Where(e => !string.IsNullOrWhiteSpace(e))
                    .Distinct())
                {
                    var capturedEmail = email;
                    _emailQueue.Enqueue(() =>
                        _emailService.SendDocumentsUploadedAsync(
                            to: capturedEmail,
                            userName: uploaderEmail,
                            requestId: requestNumber,
                            administrativeUnit: administrativeUnit,
                            requestDescription: requestDescription,
                            date: DateTime.UtcNow,
                            documentsList: documentList
                        )
                    );
                }
            }

            foreach (var userIdNotif in reviewerUserIds.Where(id => id > 0))
            {
                await _mediator.Send(
                    new CreateNotificationCommand(
                        userIdNotif,
                        "Documentos cargados",
                        $"Se cargaron {entities.Count} documento(s) en la solicitud {requestNumber}",
                        request.RequestId
                    ),
                    cancellationToken
                );
            }

            return entities.Select(x => x.Id).ToList();
        }
    }
}