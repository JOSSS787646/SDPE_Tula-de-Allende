using MediatR;
using SistemaDigitalizacionPolizas.Domain.Entities.PaymentPolicy_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Commands.CreatePaymentPolicy
{
    public class CreatePaymentPolicyCommandHandler
        : IRequestHandler<CreatePaymentPolicyCommand, int>
    {
        private readonly IPaymentPolicyRepository _paymentPolicyRepository;
        private readonly IUnitOfWorkService _unitOfWork;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUserService;

        public CreatePaymentPolicyCommandHandler(
            IPaymentPolicyRepository paymentPolicyRepository,
            IUnitOfWorkService unitOfWork,
            IFileStorageService fileStorageService,
            ICurrentUserService currentUserService)
        {
            _paymentPolicyRepository = paymentPolicyRepository;
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _currentUserService = currentUserService;
        }

        public async Task<int> Handle(
            CreatePaymentPolicyCommand request,
            CancellationToken cancellationToken)
        {
            string? filePath = null;

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // 1️⃣ Validar si ya existe la póliza
                var existingPolicy = await _paymentPolicyRepository
                    .GetByPolicyCodeAsync(request.PolicyCode);

                if (existingPolicy != null)
                    throw new Exception("A policy with this code already exists.");

                // 2️⃣ Validar archivo
                if (request.File == null || request.File.Length == 0)
                    throw new Exception("File is required.");

                if (request.File.ContentType != "application/pdf")
                    throw new Exception("Only PDF files are allowed.");

                // 3️⃣ Subir archivo a Wasabi
                using (var stream = request.File.OpenReadStream())
                {
                    filePath = await _fileStorageService.UploadAsync(
    stream,
    request.File.FileName,
    request.File.ContentType,
    $"payment-policies/"
);
                }

                // 4️⃣ Obtener usuario autenticado
                var userId = _currentUserService.UserId;

                // 5️⃣ Crear entidad
                var policy = new PaymentPolicy
                {
                    PolicyCode = request.PolicyCode,
                    Description = request.Description,
                    FilePath = filePath,
                    CreatedBy = userId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                // 6️⃣ Guardar metadatos
                await _paymentPolicyRepository.CreateAsync(policy);

                // 7️⃣ Confirmar transacción
                await _unitOfWork.CommitAsync();

                return policy.IdPaymentPolicy;
            }
            catch
            {
                // 🔥 eliminar archivo si falló BD
                if (!string.IsNullOrEmpty(filePath))
                {
                    await _fileStorageService.DeleteAsync(filePath);
                }

                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}