namespace SistemaDigitalizacionPolizas.Application.Services.PasswordReset_Services.Commands.ValidateResetCode
{
    public class ValidateResetCodeCommandHandler
     : IRequestHandler<ValidateResetCodeCommand, bool>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordResetRepository _passwordResetRepository;

        public ValidateResetCodeCommandHandler(
            IUserRepository userRepository,
            IPasswordResetRepository passwordResetRepository)
        {
            _userRepository = userRepository;
            _passwordResetRepository = passwordResetRepository;
        }

        public async Task<bool> Handle(
            ValidateResetCodeCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user == null)
                return false;

            var token = await _passwordResetRepository
                .GetValidTokenAsync(user.IdUser, request.Code);

            return token != null;
        }
    }
}
