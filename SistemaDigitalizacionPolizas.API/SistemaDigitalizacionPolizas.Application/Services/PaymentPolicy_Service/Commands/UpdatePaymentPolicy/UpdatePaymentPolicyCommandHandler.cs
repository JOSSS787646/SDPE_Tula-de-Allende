using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Commands.UpdatePaymentPolicy
{
    public class UpdatePaymentPolicyCommandHandler
     : IRequestHandler<UpdatePaymentPolicyCommand, bool>
    {
        private readonly IPaymentPolicyRepository _repository;
        private readonly IFileStorageService _fileStorage;
        private readonly IUnitOfWorkService _unitOfWork;

        public UpdatePaymentPolicyCommandHandler(
            IPaymentPolicyRepository repository,
            IFileStorageService fileStorage,
            IUnitOfWorkService unitOfWork)
        {
            _repository = repository;
            _fileStorage = fileStorage;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(
            UpdatePaymentPolicyCommand request,
            CancellationToken cancellationToken)
        {
            var policy = await _repository.GetByIdAsync(request.IdPaymentPolicy);

            if (policy == null)
                throw new Exception("La póliza no existe.");

            string? oldFilePath = policy.FilePath;
            string? newFilePath = null;

            try
            {
                // ================================
                // Subir nueva imagen si existe
                // ================================
                if (request.File != null)
                {
                    using var stream = request.File.OpenReadStream();

                    newFilePath = await _fileStorage.UploadAsync(
                        stream,
                        request.File.FileName,
                        request.File.ContentType,
                        "payment-policies");
                }

                // ================================
                // Actualizar datos
                // ================================
                policy.PolicyCode = request.PolicyCode;
                policy.Description = request.Description;

                if (newFilePath != null)
                    policy.FilePath = newFilePath;


                await _repository.UpdateAsync(policy);

                await _unitOfWork.SaveChangesAsync();

                // ================================
                // Eliminar archivo viejo
                // ================================
                if (newFilePath != null && !string.IsNullOrEmpty(oldFilePath))
                    await _fileStorage.DeleteAsync(oldFilePath);

                return true;
            }
            catch
            {
                // ================================
                // Si falla BD eliminar archivo nuevo
                // ================================
                if (newFilePath != null)
                    await _fileStorage.DeleteAsync(newFilePath);

                throw;
            }
        }
    }
}
