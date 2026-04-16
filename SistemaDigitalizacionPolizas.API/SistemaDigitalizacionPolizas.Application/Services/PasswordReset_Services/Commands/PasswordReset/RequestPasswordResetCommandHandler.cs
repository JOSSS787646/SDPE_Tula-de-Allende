using SistemaDigitalizacionPolizas.Domain.Entities.Auth_Entities;
using SistemaDigitalizacionPolizas.Domain.Interfaces.Services;

namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.PasswordReset
{
    public class RequestPasswordResetCommandHandler
       : IRequestHandler<RequestPasswordResetCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetRepository _resetRepository;
        private readonly IEmailService _emailService;

        public RequestPasswordResetCommandHandler(
            IUserRepository userRepository,
            IPasswordResetRepository resetRepository,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _resetRepository = resetRepository;
            _emailService = emailService;
        }

        public async Task<bool> Handle(
            RequestPasswordResetCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            // 🔐 Seguridad: no revelamos si existe o no
            if (user == null)
                return true;

            var code = new Random().Next(100000, 999999).ToString();

            var token = new PasswordResetToken
            {
                IdUser = user.IdUser,
                Code = code,
                ExpirationDate = DateTime.UtcNow.AddMinutes(10),
                Used = false
            };

            await _resetRepository.CreateAsync(token);

            await _emailService.SendAsync(
                user.Email,
                "Recuperación de contraseña",
                $"Tu código de recuperación es: {code}"
            );

            return true;
        }
    }
}
