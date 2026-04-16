using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

using SistemaDigitalizacionPolizas.Application.Services.Notification_Service.Commands.CreateNotification;

namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.AppendExpedientDocuments
{
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
        private readonly IMediator _mediator;

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
            IEmailQueue emailQueue,
            IMediator mediator)
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
            _mediator = mediator;
        }

        public async Task<List<int>> Handle(
            AppendExpedientDocumentsCommand request,
            CancellationToken cancellationToken)
        {
            // ================================
            // VALIDACIONES DE ENTRADA
            // ================================
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
                // ================================
                // OBTENER ESTADO BASE (CARGADO)
                // ================================
                var cargadoStatus = await _statusRepository
                    .GetByCodeAsync((int)DocumentStatusEnum.Cargado);

                if (cargadoStatus == null)
                    throw new Exception("No existe estado 'Cargado' configurado.");

                // ================================
                // SUBIDA Y CREACIÓN DE ENTIDADES
                // ================================
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

                // ================================
                // PERSISTENCIA
                // ================================
                await _repository.AddRangeAsync(entities);

                // Importante: guardar antes de recalcular estado
                await _unitOfWork.SaveChangesAsync();

                // ================================
                // REGLA DE NEGOCIO:
                // RECALCULAR ESTADO DE LA SOLICITUD
                // ================================
                await _requestStatusService.RecalculateStatus(request.RequestId);

                await _unitOfWork.CommitAsync();

                // ================================
                // OBTENER DATOS PARA NOTIFICACIONES
                // ================================

                // *** CORRECCIÓN PRINCIPAL ***
                // Se obtienen TODOS los emails e IDs del rol ReadView, no solo uno
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
                var administrativeUnitName = requestInfo?.AdministrativeUnitName ?? "No especificada";
                var requestDescription = requestInfo?.Justification ?? "Sin justificación";

                // ================================
                // ENVÍO DE CORREO (CONDICIONAL)
                // Se itera sobre TODOS los revisores con el rol
                // ================================
                var shouldSendEmail =
                    await _notificationPolicyService
                        .ShouldSendNotificationAsync(request.RequestId);

                if (shouldSendEmail && reviewerEmails.Any())
                {
                    foreach (var email in reviewerEmails.Where(e => !string.IsNullOrEmpty(e)))
                    {
                        // Captura local para evitar closure bug en el loop
                        var capturedEmail = email;

                        _emailQueue.Enqueue(() =>
                            _emailService.SendDocumentsUploadedAsync(
                                capturedEmail,
                                _currentUserService.Email,
                                requestNumber,
                                administrativeUnitName,
                                requestDescription,
                                DateTime.UtcNow,
                                documentList
                            )
                        );
                    }
                }

                // ================================
                // NOTIFICACIÓN IN-APP
                // Se itera sobre TODOS los revisores con el rol
                // ================================
                foreach (var reviewerUserId in reviewerUserIds.Where(id => id > 0))
                {
                    await _mediator.Send(
                        new CreateNotificationCommand(
                            reviewerUserId,
                            "Documentos agregados",
                            $"Se agregaron {entities.Count} documento(s) a la solicitud {requestNumber}",
                            request.RequestId
                        ),
                        cancellationToken
                    );
                }

                return entities.Select(x => x.Id).ToList();
            }
            catch
            {
                // ================================
                // ROLLBACK + LIMPIEZA DE ARCHIVOS
                // ================================
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