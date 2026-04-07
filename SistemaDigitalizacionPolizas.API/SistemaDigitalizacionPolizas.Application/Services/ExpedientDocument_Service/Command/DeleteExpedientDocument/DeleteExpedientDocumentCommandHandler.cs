using MediatR;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Document;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;


namespace SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Command.DeleteExpedientDocument
{
    public class DeleteExpedientDocumentCommandHandler
        : IRequestHandler<DeleteExpedientDocumentCommand, bool>
    {
        private readonly IDocumentExpedientRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IRequestStatusService _requestStatusService;
        private readonly IUnitOfWorkService _unitOfWork;
        private readonly ICurrentUserService _currentUser;

        public DeleteExpedientDocumentCommandHandler(
            IDocumentExpedientRepository repository,
            IUserRepository userRepository,
            IFileStorageService fileStorageService,
            IRequestStatusService requestStatusService,
            IUnitOfWorkService unitOfWork,
            ICurrentUserService currentUser)
        {
            _repository = repository;
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _requestStatusService = requestStatusService;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            DeleteExpedientDocumentCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("Usuario no encontrado.");

            var validPassword = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.Password
            );

            if (!validPassword)
                throw new Exception("Contraseña incorrecta.");

            var entity = await _repository.GetByIdAsync(request.Id);

            if (entity == null)
                throw new Exception("Documento no encontrado.");

            var filePath = entity.FilePath;
            var requestId = entity.RequestId;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // ==========================================
                // 1. Eliminar documento
                // ==========================================
                _repository.Delete(entity);

                // 🔥 IMPORTANTE: persistir antes de recalcular
                await _unitOfWork.SaveChangesAsync();

                // ==========================================
                // 2. Recalcular estado
                // ==========================================
                await _requestStatusService.RecalculateStatus(requestId);

                // ==========================================
                // 3. Confirmar transacción
                // ==========================================
                await _unitOfWork.CommitAsync();

                // ==========================================
                // 4. Eliminar archivo físico (fuera de transacción)
                // ==========================================
                if (!string.IsNullOrEmpty(filePath))
                    await _fileStorageService.DeleteAsync(filePath);

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}