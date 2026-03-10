using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.Acquisition;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Repositories.IPaymentPolicy;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;
using SistemaDigitalizacionPolizas.Infrastructure.Persistence.Services.UploatFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaDigitalizacionPolizas.Application.Services.PaymentPolicy_Service.Commands.DeletePaymentPolicy
{
    public class DeletePaymentPolicyCommandHandler
    : IRequestHandler<DeletePaymentPolicyCommand, bool>
    {
        private readonly IPaymentPolicyRepository _policyRepository;
        private readonly IAcquisitionRequest _requestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUser;

        public DeletePaymentPolicyCommandHandler(
            IPaymentPolicyRepository policyRepository,
            IAcquisitionRequest requestRepository,
            IUserRepository userRepository,
            IFileStorageService fileStorageService,
            ICurrentUserService currentUser)
        {
            _policyRepository = policyRepository;
            _requestRepository = requestRepository;
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(
            DeletePaymentPolicyCommand request,
            CancellationToken cancellationToken)
        {
            // 1️⃣ Usuario actual
            var userId = _currentUser.UserId;

            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                throw new Exception("Usuario no encontrado");

            // 2️⃣ Validar contraseña
            var validPassword = BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.Password
            );

            if (!validPassword)
                throw new Exception("Contraseña incorrecta");

            // 3️⃣ Obtener póliza
            var policy = await _policyRepository.GetByIdAsync(request.IdPaymentPolicy);

            if (policy == null)
                throw new Exception("La póliza no existe");

            // 4️⃣ Desvincular póliza de solicitudes
            await _requestRepository.RemovePaymentPolicyFromRequests(request.IdPaymentPolicy);

            // 5️⃣ Eliminar archivo en Wasabi
            if (!string.IsNullOrEmpty(policy.FilePath))
            {
                await _fileStorageService.DeleteAsync(policy.FilePath);
            }

            // 6️⃣ Desactivar o eliminar póliza
            await _policyRepository.DeleteAsync(request.IdPaymentPolicy);

            return true;
        }
    }
}