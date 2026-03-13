using MediatR;
using Microsoft.Extensions.Logging;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Domain.Entities.Document_Entities;
using SistemaDigitalizacionPolizas.Domain.Enums;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.StatusRequest;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.RequestNotification;
using System.Threading;

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
        private readonly ILogger<CreateMassiveExpedientDocumentCommandHandler> _logger;

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
            ILogger<CreateMassiveExpedientDocumentCommandHandler> logger)
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
            _logger = logger;
        }

        public async Task<List<int>> Handle(
            CreateMassiveExpedientDocumentCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando carga masiva de documentos para RequestId {RequestId}", request.RequestId);

            if (request.Documents == null || !request.Documents.Any())
                throw new Exception("Debe enviar al menos un archivo.");

            if (request.Documents.Count > 100)
                throw new Exception("Máximo 100 archivos permitidos por carga.");

            var uploadedPaths = new List<string>();
            var entities = new List<ExpedientDocument>();
            var userId = _currentUserService.UserId;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var cargadoStatus = await _statusRepository
                    .GetByCodeAsync((int)DocumentStatusEnum.Cargado);

                if (cargadoStatus == null)
                    throw new Exception("No existe estado 'Cargado' configurado.");

                // ============================================
                // SUBIDA DE ARCHIVOS EN PARALELO
                // ============================================

                var semaphore = new SemaphoreSlim(3); // máximo 3 uploads simultáneos

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
                        {
                            uploadedPaths.Add(filePath);
                        }

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
                        {
                            entities.Add(entity);
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });

                await Task.WhenAll(tasks);

                _logger.LogInformation("Archivos subidos correctamente para RequestId {RequestId}", request.RequestId);

                // ============================================
                // GUARDAR EN BASE DE DATOS
                // ============================================

                await _repository.AddRangeAsync(entities);

                await _requestStatusService.RecalculateStatus(request.RequestId);

                await _unitOfWork.CommitAsync();

                _logger.LogInformation("Documentos guardados correctamente para RequestId {RequestId}", request.RequestId);

                // ============================================
                // VALIDAR ENVÍO DE CORREO
                // ============================================

                var shouldSendNotification = await _notificationPolicyService
                    .ShouldSendNotificationAsync(request.RequestId);

                if (!shouldSendNotification)
                {
                    _logger.LogInformation("La política de notificaciones bloqueó el envío de correo para RequestId {RequestId}", request.RequestId);
                    return entities.Select(x => x.Id).ToList();
                }

                var reviewerEmail = await _userRepository
                    .GetEmailByRoleAsync((int)SystemRolesEnum.ReadView);

                if (string.IsNullOrEmpty(reviewerEmail))
                {
                    _logger.LogWarning("No se encontró correo para el rol ReadView.");
                    return entities.Select(x => x.Id).ToList();
                }

                var requestInfo = await _notificationRepository
                    .GetRequestNotificationInfoAsync(request.RequestId);

                var documentList = string.Join("",
                    entities.Select(x => $"<div>• {x.FileName}</div>")
                );

                var requestNumber = requestInfo?.RequestNumber ?? "N/A";
                var administrativeUnitName = requestInfo?.AdministrativeUnitName ?? "No especificada";
                var requestDescription = requestInfo?.Justification ?? "Sin justificación";

                _logger.LogInformation("Enviando correo de notificación a {Email}", reviewerEmail);

                await _emailService.SendDocumentsUploadedAsync(
                    reviewerEmail,
                    _currentUserService.Email,
                    requestNumber,
                    administrativeUnitName,
                    requestDescription,
                    DateTime.Now,
                    documentList
                );

                _logger.LogInformation("Correo enviado correctamente para RequestId {RequestId}", request.RequestId);

                return entities.Select(x => x.Id).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la carga masiva de documentos para RequestId {RequestId}", request.RequestId);

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